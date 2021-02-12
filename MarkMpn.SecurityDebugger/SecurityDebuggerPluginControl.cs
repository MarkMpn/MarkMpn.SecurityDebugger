using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using ScintillaNET;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MarkMpn.SecurityDebugger
{
    public partial class SecurityDebuggerPluginControl : PluginControlBase, IGitHubPlugin, IAboutPlugin, IHelpPlugin
    {
        private EntityReference _principalReference;
        private EntityReference _targetReference;

        public SecurityDebuggerPluginControl()
        {
            InitializeComponent();

            scintilla1.Lexer = Lexer.Null;
            scintilla1.StyleResetDefault();
            scintilla1.Styles[Style.Default].Font = "Courier New";
            scintilla1.Styles[Style.Default].Size = 10;
            scintilla1.StyleClearAll();

            scintilla1.Styles[1].BackColor = Color.Yellow;
        }

        private void ParseError()
        {
            noMatchPanel.Visible = true;
            recordPermissionsPanel.Visible = false;
            errorPanel.Visible = false;
            retryPanel.Visible = false;

            scintilla1.StartStyling(0);
            scintilla1.SetStyling(scintilla1.TextLength, 0);

            if (ConnectionDetail == null)
            {
                MessageBox.Show("Please connect to an instance first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var regexes = new[]
            {
                // SecLib::AccessCheckEx failed. Returned hr = -2147187962, ObjectID: <guid>, OwnerId: <guid>,  OwnerIdType: <int> and CallingUser: <guid>. ObjectTypeCode: <int>, objectBusinessUnitId: <guid>, AccessRights: <accessrights>
                // Target: <objectid>,<objectidtype>
                // Principal: <callinguserid>
                // Privilege: <accessrights>,<objectidtype>
                // Depth: <objectid>,<callinguserid>
                new Regex("ObjectID: (?<objectid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+), OwnerId: (?<ownerid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+),  OwnerIdType: (?<owneridtype>[0-9]+) and CallingUser: (?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+). ObjectTypeCode: (?<objectidtype>[0-9]+), objectBusinessUnitId: (?<objectbusinessunitid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+), AccessRights: (?<accessrights>[a-z]+)", RegexOptions.IgnoreCase),
                
                // SecLib::CheckPrivilege failed. User: <guid>, PrivilegeName: <prvName>, PrivilegeId: <guid>, Required Depth: <depth>, BusinessUnitId: <guid>, MetadataCache Privileges Count: <int>, User Privileges Count: <int>
                // Target: <privilegename>
                // Principal: <userid>
                // Privilege: <privilegename>
                // Depth: <privilegedepth>
                new Regex("User: (?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+), PrivilegeName: (?<privilegename>prv[a-z0-9_]+), PrivilegeId: (?<privilegeid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+), Required Depth: (?<privilegedepth>Basic|Local|Deep|Global)", RegexOptions.IgnoreCase),
                
                // Principal user (Id=<guid>, type=<int>, roleCount=<int>, privilegeCount=<int>, accessMode=<int>), is missing <prvName> privilege (Id=<guid>) on OTC=<int> for entity '<logicalname>'. context.Caller=<guid>. Or identityUser.SystemUserId=<guid>, identityUser.Privileges.Count=<int>, identityUser.Roles.Count=<int> is missing <prvName> privilege (Id=<guid>) on OTC=<int> for entity '<logicalname>'.
                // Target: <logicalname>
                // Principal: <userid>
                // Privilege: <privilegename>
                // Depth: Basic
                new Regex("Principal user \\(Id=\\s*(?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+)(\\s*,\\s*type=(?<usertype>[0-9]+))?(\\s*,\\s*roleCount=[0-9]+)?(\\s*,\\s*privilegeCount=[0-9]+)?(\\s*,\\s*accessMode=[0-9]+)?\\)\\s*,?\\s*is missing (?<privilegename>prv[a-z0-9_]+)\\sprivilege( \\(Id=(?<privilegeid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+)\\))?( on OTC=(?<objectidtype>[0-9]+))?( for entity '(?<objectidtypename>[a-z0-9_]+)')?", RegexOptions.IgnoreCase),

                // Principal with id <guid> does not have <accessrights> right(s) for record with id <guid> of entity <entityname>
                // Target <objectid>,<objectidtype>
                // Principal: <callinguserid>
                // Privilege: <accessrights>,<objectidtype>
                // Depth: <objectid>,<callinguserid>
                new Regex("Principal with id (?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+) does not have (?<accessrights>[a-z]+) right\\(s\\) for record with id (?<objectid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+) of entity (?<objectidtype>[a-z0-9_]+)", RegexOptions.IgnoreCase)
            };

            foreach (var regex in regexes)
            {
                var match = regex.Match(scintilla1.Text);

                if (match.Success)
                {
                    foreach (Group group in match.Groups)
                    {
                        if (Int32.TryParse(group.Name, out _))
                            continue;

                        scintilla1.StartStyling(group.Index);
                        scintilla1.SetStyling(group.Length, 1);
                    }

                    WorkAsync(new WorkAsyncInfo
                    {
                        Message = "Extracting Details...",
                        Work = (bw, args) =>
                        {
                            try
                            {
                                // Extract the details from the error message
                                _principalReference = ExtractPrincipal(match, out var principalTypeDisplayName);
                                var target = ExtractTarget(match, out var targetTypeDisplayName);
                                var privilege = ExtractPrivilege(match, target);
                                var privilegeDepth = ExtractPrivilegeDepth(match, _principalReference, target);

                                var accessRights = (AccessRights)privilege.GetAttributeValue<int>("accessright");
                                _targetReference = target as EntityReference;
                                var privilegeRef = privilege.ToEntityReference();
                                privilegeRef.Name = privilege.GetAttributeValue<string>("name");

                                // Check if the problem should still exist
                                try
                                {
                                    if (_targetReference != null && accessRights != AccessRights.None)
                                    {
                                        var recordAccess = (RetrievePrincipalAccessResponse)Service.Execute(new RetrievePrincipalAccessRequest
                                        {
                                            Principal = _principalReference,
                                            Target = _targetReference
                                        });

                                        if ((recordAccess.AccessRights & accessRights) == accessRights)
                                            InvokeIfRequired(() => retryPanel.Visible = true);
                                    }
                                    else
                                    {
                                        var priv = (RetrieveUserPrivilegeByPrivilegeIdResponse)Service.Execute(new RetrieveUserPrivilegeByPrivilegeIdRequest
                                        {
                                            UserId = _principalReference.Id,
                                            PrivilegeId = privilege.Id
                                        });

                                        if (priv.RolePrivileges.Any())
                                            InvokeIfRequired(() => retryPanel.Visible = true);
                                    }
                                }
                                catch (FaultException<OrganizationServiceFault>)
                                {
                                    // In case the service doesn't support the RetrieveUserPrivilegeByPrivilegeId message
                                }

                                // Display the problem details
                                InvokeIfRequired(() =>
                                {
                                    var userPrefix = $"The {principalTypeDisplayName} ";
                                    var userName = _principalReference.Name;
                                    userLinkLabel.Text = userPrefix + userName;
                                    userLinkLabel.LinkArea = new LinkArea(userPrefix.Length, userName.Length);

                                    if (accessRights == AccessRights.None)
                                        missingPrivilegeLinkLabel.Text = $"does not have {privilege.GetAttributeValue<string>("name")} permission";
                                    else
                                        missingPrivilegeLinkLabel.Text = $"does not have {accessRights.ToString().Replace("Access", "")} permission";

                                    var prefix = $"on the {targetTypeDisplayName} ";
                                    var link = "";

                                    if (_targetReference != null)
                                        link = _targetReference.Name ?? "<Unnamed>";
                                    else
                                        prefix += ((EntityMetadata)target).DisplayName.UserLocalizedLabel.Label;

                                    targetLinkLabel.Text = prefix + link;
                                    targetLinkLabel.LinkArea = new LinkArea(prefix.Length, link.Length);

                                    // Check permission is available at the minimum calculated depth. If not, increase the depth
                                    // to the next available value.
                                    if (privilegeDepth == PrivilegeDepth.Basic && !privilege.GetAttributeValue<bool>("canbebasic"))
                                        privilegeDepth = PrivilegeDepth.Local;

                                    if (privilegeDepth == PrivilegeDepth.Local && !privilege.GetAttributeValue<bool>("canbelocal"))
                                        privilegeDepth = PrivilegeDepth.Deep;

                                    if (privilegeDepth == PrivilegeDepth.Deep && !privilege.GetAttributeValue<bool>("canbedeep"))
                                        privilegeDepth = PrivilegeDepth.Global;

                                    requiredPrivilegeLabel.Text = $"To resolve this error, the user needs to be granted the {privilegeRef.Name} privilege to {privilegeDepth} depth";

                                    switch (privilegeDepth)
                                    {
                                        case PrivilegeDepth.Basic:
                                            requiredDepthImage.Image = Properties.Resources.Basic;
                                            break;

                                        case PrivilegeDepth.Local:
                                            requiredDepthImage.Image = Properties.Resources.Local;
                                            break;

                                        case PrivilegeDepth.Deep:
                                            requiredDepthImage.Image = Properties.Resources.Deep;
                                            break;

                                        case PrivilegeDepth.Global:
                                            requiredDepthImage.Image = Properties.Resources.Global;
                                            break;
                                    }
                                });

                                var resolutions = new List<Resolution>();

                                // Find roles that include the required permission and suggest to add them to the user
                                var depthQuery = 2 ^ (int) privilegeDepth;

                                var sufficientRoleQry = new FetchExpression($@"
                                    <fetch xmlns:generator='MarkMpn.SQL4CDS'>
                                        <entity name='role'>
                                            <attribute name='name' />
                                            <link-entity name='roleprivileges' to='roleid' from='roleid' alias='rp' link-type='inner'>
                                                <filter>
                                                    <condition attribute='privilegeid' operator='eq' value='{privilege.Id}' />
                                                    <condition attribute='privilegedepthmask' operator='ge' value='{depthQuery}' />
                                                </filter>
                                            </link-entity>
                                            <order attribute='name' />
                                        </entity>
                                    </fetch>");
                                var sufficientRoles = Service.RetrieveMultiple(sufficientRoleQry);

                                foreach (var role in sufficientRoles.Entities)
                                {
                                    var roleRef = role.ToEntityReference();
                                    roleRef.Name = role.GetAttributeValue<string>("name");

                                    var addRole = new AddSecurityRole
                                    {
                                        UserReference = _principalReference,
                                        RoleReference = roleRef
                                    };

                                    resolutions.Add(addRole);
                                }

                                // Find roles currently assigned to the user and suggest them to be edited to include the required permission
                                var existingRoleQry = new FetchExpression($@"
                                    <fetch xmlns:generator='MarkMpn.SQL4CDS'>
                                        <entity name='role'>
                                            <attribute name='name' />
                                            <link-entity name='roleprivileges' to='roleid' from='roleid' alias='rp' link-type='outer'>
                                                <attribute name='privilegedepthmask' />
                                                <filter>
                                                    <condition attribute='privilegeid' operator='eq' value='{privilege.Id}' />
                                                </filter>
                                            </link-entity>
                                            <link-entity name='systemuserroles' to='roleid' from='roleid' alias='sur' link-type='inner'>
                                                <filter>
                                                    <condition attribute='systemuserid' operator='eq' value='{_principalReference.Id}' />
                                                </filter>
                                            </link-entity>
                                            <filter type='or'>
                                                <condition entityname='rp' attribute='privilegeid' operator='null' />
                                                <condition entityname='rp' attribute='privilegedepthmask' operator='lt' value='{depthQuery}' />
                                            </filter>
                                            <order attribute='name' />
                                        </entity>
                                    </fetch>");
                                var existingRoles = Service.RetrieveMultiple(existingRoleQry);

                                foreach (var role in existingRoles.Entities)
                                {
                                    var roleRef = role.ToEntityReference();
                                    roleRef.Name = role.GetAttributeValue<string>("name");
                                    var existingDepth = role.GetAttributeValue<AliasedValue>("rp.privilegedepthmask");

                                    var editRole = new EditSecurityRole
                                    {
                                        RoleReference = roleRef,
                                        PrivilegeReference = privilegeRef,
                                        Depth = privilegeDepth,
                                        ExistingDepth = existingDepth == null ? (PrivilegeDepth?)null : (PrivilegeDepth) Math.Log((int)existingDepth.Value, 2)
                                    };

                                    resolutions.Add(editRole);
                                }

                                if (_targetReference != null)
                                {
                                    // Suggest sharing the record with the required permission
                                    var sharing = new ShareRecord
                                    {
                                        UserReference = _principalReference,
                                        TargetReference = _targetReference,
                                        AccessRights = accessRights
                                    };

                                    resolutions.Add(sharing);
                                }

                                InvokeIfRequired(() =>
                                {
                                    resolutionsListView.Items.Clear();

                                    foreach (var resolution in resolutions)
                                    {
                                        var lvi = resolutionsListView.Items.Add(resolution.ToString());
                                        lvi.ImageIndex = resolution.ImageIndex;
                                        lvi.Tag = resolution;
                                    }

                                    noMatchPanel.Visible = false;
                                    recordPermissionsPanel.Visible = true;
                                });
                            }
                            catch (Exception ex)
                            {
                                InvokeIfRequired(() =>
                                {
                                    errorLabel.Text = ex.Message;

                                    // If this is an ObjectDoesNotExist error, it's likely that the error is from a different instance
                                    unchecked
                                    {
                                        if (ex is FaultException<OrganizationServiceFault> fault && fault.Detail.ErrorCode == (int) 0x80040217)
                                        {
                                            errorLabel.Text += "\r\n\r\nAre you connected to the same instance the error message came from?";
                                        }
                                    }

                                    noMatchPanel.Visible = false;
                                    errorPanel.Visible = true;
                                });
                            }
                        }
                    });
                }
            }
        }

        private void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private PrivilegeDepth ExtractPrivilegeDepth(Match match, EntityReference principal, object target)
        {
            if (match.Groups["privilegedepth"].Success)
                return (PrivilegeDepth)Enum.Parse(typeof(PrivilegeDepth), match.Groups["privilegedepth"].Value);

            if (!(target is EntityReference entity))
                return PrivilegeDepth.Basic;

            var metadata = ((RetrieveEntityResponse)Service.Execute(new RetrieveEntityRequest
            {
                LogicalName = entity.LogicalName,
                EntityFilters = EntityFilters.Entity
            })).EntityMetadata;

            if (metadata.OwnershipType == OwnershipTypes.OrganizationOwned)
                return PrivilegeDepth.Global;

            var entityWithOwner = Service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet("ownerid"));
            var ownerRef = entityWithOwner.GetAttributeValue<EntityReference>("ownerid");
            var owner = Service.Retrieve(ownerRef.LogicalName, ownerRef.Id, new ColumnSet("businessunitid"));
            var buRef = owner.GetAttributeValue<EntityReference>("businessunitid");

            if (ownerRef.Id == principal.Id)
                return PrivilegeDepth.Basic;

            var me = Service.Retrieve(principal.LogicalName, principal.Id, new ColumnSet("businessunitid"));
            var myBusinessUnitId = me.GetAttributeValue<EntityReference>("businessunitid");

            if (buRef.Id == myBusinessUnitId.Id)
                return PrivilegeDepth.Local;

            // Business Unit doesn't have a hierarchical relationship so we can't do a simple query to see
            // if the target business unit is under the user's business unit, so recurse up from the target
            // business unit instead to see if we reach the user's business unit
            var bu = Service.Retrieve(buRef.LogicalName, buRef.Id, new ColumnSet("parentbusinessunitid"));
            var parentBuRef = bu.GetAttributeValue<EntityReference>("parentbusinessunitid");

            while (parentBuRef != null)
            {
                if (parentBuRef.Id == myBusinessUnitId.Id)
                    return PrivilegeDepth.Deep;

                bu = Service.Retrieve(parentBuRef.LogicalName, parentBuRef.Id, new ColumnSet("parentbusinessunitid"));
                parentBuRef = bu.GetAttributeValue<EntityReference>("parentbusinessunitid");
            }

            return PrivilegeDepth.Global;
        }

        private Entity ExtractPrivilege(Match match, object target)
        {
            if (match.Groups["privilegename"].Success)
            {
                var privQry = new QueryByAttribute("privilege");
                privQry.AddAttributeValue("name", match.Groups["privilegename"].Value);
                privQry.ColumnSet = new ColumnSet("name", "accessright", "canbebasic", "canbelocal", "canbedeep", "canbeglobal");
                return Service.RetrieveMultiple(privQry).Entities.Single();
            }

            if (match.Groups["accessrights"].Success)
            {
                string targetType;

                if (target is EntityReference e)
                    targetType = e.LogicalName;
                else if (target is EntityMetadata m)
                    targetType = m.LogicalName;
                else
                    throw new NotSupportedException();

                var metadata = ((RetrieveEntityResponse)Service.Execute(new RetrieveEntityRequest
                {
                    LogicalName = targetType,
                    EntityFilters = EntityFilters.Privileges
                })).EntityMetadata;

                var accessRights = (AccessRights)Enum.Parse(typeof(AccessRights), match.Groups["accessrights"].Value);

                var accessRightsToPrivType = new Dictionary<AccessRights, PrivilegeType>
                {
                    [AccessRights.AppendAccess] = PrivilegeType.Append,
                    [AccessRights.AppendToAccess] = PrivilegeType.AppendTo,
                    [AccessRights.AssignAccess] = PrivilegeType.Assign,
                    [AccessRights.CreateAccess] = PrivilegeType.Create,
                    [AccessRights.DeleteAccess] = PrivilegeType.Delete,
                    [AccessRights.ReadAccess] = PrivilegeType.Read,
                    [AccessRights.ShareAccess] = PrivilegeType.Share,
                    [AccessRights.WriteAccess] = PrivilegeType.Write
                };

                if (!accessRightsToPrivType.TryGetValue(accessRights, out var privType))
                    throw new NotSupportedException();

                var priv = metadata.Privileges.Single(p => p.PrivilegeType == privType);
                return Service.Retrieve("privilege", priv.PrivilegeId, new ColumnSet("name", "accessright", "canbebasic", "canbelocal", "canbedeep", "canbeglobal"));
            }

            throw new NotSupportedException();
        }

        private object ExtractTarget(Match match, out string targetTypeDisplayName)
        {
            if (match.Groups["objectid"].Success && match.Groups["objectidtype"].Success)
            {
                var objectTypeKey = nameof(EntityMetadata.LogicalName);
                var objectType = (object)match.Groups["objectidtype"].Value;

                if (Int32.TryParse(match.Groups["objectidtype"].Value, out var objectTypeCode))
                {
                    objectTypeKey = nameof(EntityMetadata.ObjectTypeCode);
                    objectType = objectTypeCode;
                }

                var targetEntityQuery = new RetrieveMetadataChangesRequest
                {
                    Query = new EntityQueryExpression
                    {
                        Criteria = new MetadataFilterExpression
                        {
                            Conditions =
                            {
                                new MetadataConditionExpression(objectTypeKey, MetadataConditionOperator.Equals, objectType)
                            }
                        },
                        Properties = new MetadataPropertiesExpression
                        {
                            PropertyNames =
                            {
                                nameof(EntityMetadata.LogicalName),
                                nameof(EntityMetadata.PrimaryNameAttribute),
                                nameof(EntityMetadata.DisplayName),
                                nameof(EntityMetadata.SchemaName)
                            }
                        }
                    }
                };
                var targetEntityResults = (RetrieveMetadataChangesResponse)Service.Execute(targetEntityQuery);
                var targetMetadata = targetEntityResults.EntityMetadata[0];

                var targetReference = new EntityReference(targetMetadata.LogicalName, new Guid(match.Groups["objectid"].Value));
                var target = Service.Retrieve(targetReference.LogicalName, targetReference.Id, new ColumnSet(targetMetadata.PrimaryNameAttribute));
                targetReference.Name = target.GetAttributeValue<string>(targetMetadata.PrimaryNameAttribute);

                targetTypeDisplayName = targetMetadata.DisplayName.UserLocalizedLabel.Label;
                return targetReference;
            }
            
            if (match.Groups["objectidtypename"].Success)
            {
                var logicalName = match.Groups["objectidtypename"].Value;

                var entityQuery = new RetrieveMetadataChangesRequest
                {
                    Query = new EntityQueryExpression
                    {
                        Criteria = new MetadataFilterExpression
                        {
                            Conditions =
                            {
                                new MetadataConditionExpression(nameof(EntityMetadata.LogicalName), MetadataConditionOperator.Equals, logicalName)
                            }
                        },
                        Properties = new MetadataPropertiesExpression
                        {
                            PropertyNames =
                            {
                                nameof(EntityMetadata.DisplayName)
                            }
                        }
                    }
                };
                var entityResults = (RetrieveMetadataChangesResponse)Service.Execute(entityQuery);
                var metadata = entityResults.EntityMetadata[0];

                targetTypeDisplayName = "entity";
                return metadata;
            }

            throw new NotSupportedException("Could not find target");
        }

        private EntityReference ExtractPrincipal(Match match, out string principalTypeDisplayName)
        {
            // User ID is in the "callinguser" regex group
            var userId = new Guid(match.Groups["callinguser"].Value);
            var userTypeKey = nameof(EntityMetadata.LogicalName);

            // Assume this is a user, but could be a team. If we've got a "usertype" group, extract the logical name/object type code
            var userType = (object)"systemuser";

            if (match.Groups["usertype"].Success)
            {
                userType = match.Groups["usertype"].Value;

                if (Int32.TryParse((string) userType, out var userTypeCode))
                {
                    userTypeKey = nameof(EntityMetadata.ObjectTypeCode);
                    userType = userTypeCode;
                }
            }

            // Get the full details of the entity type of the principal
            var userEntityQuery = new RetrieveMetadataChangesRequest
            {
                Query = new EntityQueryExpression
                {
                    Criteria = new MetadataFilterExpression
                    {
                        Conditions =
                        {
                            new MetadataConditionExpression(userTypeKey, MetadataConditionOperator.Equals, userType)
                        }
                    },
                    Properties = new MetadataPropertiesExpression
                    {
                        PropertyNames =
                        {
                            nameof(EntityMetadata.LogicalName),
                            nameof(EntityMetadata.PrimaryNameAttribute),
                            nameof(EntityMetadata.DisplayName)
                        }
                    }
                }
            };

            var userEntityResults = (RetrieveMetadataChangesResponse)Service.Execute(userEntityQuery);
            var userMetadata = userEntityResults.EntityMetadata[0];

            // Get the full user details so that we can populate the display name
            var userReference = new EntityReference(userMetadata.LogicalName, userId);
            var user = Service.Retrieve(userReference.LogicalName, userReference.Id, new ColumnSet(userMetadata.PrimaryNameAttribute));
            userReference.Name = user.GetAttributeValue<string>(userMetadata.PrimaryNameAttribute);

            principalTypeDisplayName = userMetadata.DisplayName.UserLocalizedLabel.Label;

            return userReference;
        }

        abstract class Resolution
        {
            public abstract int ImageIndex { get; }

            public abstract void Execute(IOrganizationService org);
        }

        class AddSecurityRole : Resolution
        {
            public override int ImageIndex => 1;

            public EntityReference UserReference { get; set; }

            public EntityReference RoleReference { get; set; }

            public override void Execute(IOrganizationService org)
            {
                org.Associate(
                    UserReference.LogicalName,
                    UserReference.Id,
                    new Relationship($"{UserReference.LogicalName}roles_association"),
                    new EntityReferenceCollection { RoleReference });
            }

            public override string ToString()
            {
                return $"Add the {RoleReference.Name} role to {UserReference.Name}";
            }
        }

        class EditSecurityRole : Resolution
        {
            public override int ImageIndex => 0;

            public EntityReference RoleReference { get; set; }

            public EntityReference PrivilegeReference { get; set; }

            public PrivilegeDepth Depth { get; set; }

            public PrivilegeDepth? ExistingDepth { get; set; }

            public override void Execute(IOrganizationService org)
            {
                org.Execute(new AddPrivilegesRoleRequest
                {
                    RoleId = RoleReference.Id,
                    Privileges = new[]
                    {
                        new RolePrivilege
                        {
                            PrivilegeId = PrivilegeReference.Id,
                            Depth = Depth
                        }
                    }
                });
            }

            public override string ToString()
            {
                if (ExistingDepth == null)
                    return $"Add the {PrivilegeReference.Name} privilege to the {RoleReference.Name} role at {Depth} depth";

                return $"Change the depth of the {PrivilegeReference.Name} privilege in the {RoleReference.Name} role from {ExistingDepth.Value} to {Depth}";
            }
        }

        class ShareRecord : Resolution
        {
            public override int ImageIndex => 2;

            public EntityReference UserReference { get; set; }

            public EntityReference TargetReference { get; set; }

            public AccessRights AccessRights { get; set; }

            public override void Execute(IOrganizationService org)
            {
                org.Execute(new GrantAccessRequest
                {
                    PrincipalAccess = new PrincipalAccess
                    {
                        Principal = UserReference,
                        AccessMask = AccessRights
                    },
                    Target = TargetReference
                });
            }

            public override string ToString()
            {
                return $"Share the {(TargetReference.Name ?? "<Unnamed>")} record with {UserReference.Name} with {AccessRights.ToString().Replace("Access", "")} access";
            }
        }

        private void scintilla1_TextChanged(object sender, EventArgs e)
        {
            ParseError();
        }

        private void scintilla1_Enter(object sender, EventArgs e)
        {
            scintilla1.SelectAll();
        }

        private void resolutionsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            executeButton.Enabled = resolutionsListView.SelectedItems.Count == 1;
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            var resolution = (Resolution)resolutionsListView.SelectedItems[0].Tag;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Applying Fix...",
                Work = (bw, args) =>
                {
                    resolution.Execute(Service);
                },
                PostWorkCallBack = result => ParseError()
            });
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                scintilla1.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void userLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = ConnectionDetail.WebApplicationUrl + $"main.aspx?etn={_principalReference.LogicalName}&pagetype=entityrecord&id={_principalReference.Id}";
            Process.Start(url);
        }

        private void targetLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = ConnectionDetail.WebApplicationUrl + $"main.aspx?etn={_targetReference.LogicalName}&pagetype=entityrecord&id={_targetReference.Id}";
            Process.Start(url);
        }

        private void createIssueLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/MarkMpn/MarkMpn.SecurityDebugger/issues/new");
        }

        void IAboutPlugin.ShowAboutDialog()
        {
            Process.Start(((IHelpPlugin)this).HelpUrl);
        }

        string IGitHubPlugin.RepositoryName => "MarkMpn.SecurityDebugger";

        string IGitHubPlugin.UserName => "MarkMpn";

        string IHelpPlugin.HelpUrl => "https://markcarrington.dev/security-debugger/";

        private void aboutToolStripLabel_Click(object sender, EventArgs e)
        {
            ((IAboutPlugin)this).ShowAboutDialog();
        }
    }
}
