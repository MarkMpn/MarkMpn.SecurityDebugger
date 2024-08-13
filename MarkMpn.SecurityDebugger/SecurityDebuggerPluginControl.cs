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
    public partial class SecurityDebuggerPluginControl : PluginControlBase, IGitHubPlugin, IAboutPlugin, IHelpPlugin, IPayPalPlugin
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

        class Results
        {
            public bool HasAccess { get; set; }
            public string PrincipalTypeDisplayName { get; set; }
            public string TargetTypeDisplayName { get; set; }
            public object Target { get; set; }
            public bool IsCurrentUser { get; set; }
            public List<Resolution> Resolutions { get; set; }
            public Exception Exception { get; set; }
        }

        class FLSResults : Results
        {
            public string AttributeName { get; set; }
            public string Privilege { get; set; }
        }

        class RoleResults : Results
        {
            public AccessRights AccessRights { get; set; }
            public Entity Privilege { get; set; }
            public PrivilegeDepth PrivilegeDepth { get; set; }
        }

        private void ParseError()
        {
            noMatchPanel.Visible = true;
            recordPermissionsPanel.Visible = false;
            errorPanel.Visible = false;
            retryPanel.Visible = false;
            resolutionsListView.Items.Clear();

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
                new Regex("Principal user \\(Id=\\s*(?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+)(\\s*,\\s*type=(?<usertype>[0-9]+))?(\\s*,\\s*roleCount=[0-9]+)?(\\s*,\\s*privilegeCount=[0-9]+)?(\\s*,\\s*accessMode=([0-9]+|'[0-9]+[^']+'))?[^)]*\\)\\s*,?\\s*is missing (?<privilegename>prv[a-z0-9_]+)\\sprivilege( \\(Id=(?<privilegeid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+)\\))?( on OTC=(?<objectidtype>[0-9]+))?( for entity '(?<objectidtypename>[a-z0-9_]+)')?", RegexOptions.IgnoreCase),

                // Principal with id <guid> does not have <accessrights> right(s) for record with id <guid> of entity <entityname>
                // Target <objectid>,<objectidtype>
                // Principal: <callinguserid>
                // Privilege: <accessrights>,<objectidtype>
                // Depth: <objectid>,<callinguserid>
                new Regex("Principal with id (?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+) does not have (?<accessrights>[a-z]+) right\\(s\\) for record with id (?<objectid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+) of entity (?<objectidtype>[a-z0-9_]+)(.*\"ObjectBusinessUnitId\":\"(?<businessunitid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+)\")?", RegexOptions.IgnoreCase),

                // RoleService::VerifyCallerPrivileges failed. User: <guid>, UserBU: <guid>, PrivilegeName: <privilegename>, PrivilegeId: <guid>, Depth: <depth>, BusinessUnitId: <guid>, MissingPrivilegeCount: 16
                // Target: None
                // Principal: <userid>
                // Privilege: <privilegename>
                // Depth: <privilegedepth>
                new Regex("User: (?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+), UserBU: ([a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+), PrivilegeName: (?<privilegename>prv[a-z0-9_]+), PrivilegeId: (?<privilegeid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+), Depth: (?<privilegedepth>Basic|Local|Deep|Global)", RegexOptions.IgnoreCase),

                // SecLib::CrmCheckPrivilege failed. Returned hr = -2147220839 on UserId: <guid> and Privilege: <prvName> in BusinessUnit: <businessunitname> (Id= <businessunitid>)
                // Target: <privilegename>
                // Principal: <userid>
                // Privilege: <privilegename>
                // Depth: <businessunitid>,<userid>
                new Regex("UserId: (?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+) and Privilege: (?<privilegename>prv[a-z0-9+]+) in BusinessUnit: .*? \\(Id= (?<businessunitid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+)\\)", RegexOptions.IgnoreCase),

                // User with ID <guid> does not have <flsprivilegename> permissions for the <attributename> attribute in the <entityname> entity. The <primaryid> of the record is <guid>
                // Target: <objectid>,<objectidtype>
                // Principal: <callinguserid>
                // FLS Privilege: <flsprivilegename>
                new Regex("User with ID (?<callinguser>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+) does not have (?<flsprivilegename>(Create|Read|Update)) permissions for the (?<attributename>[a-z0-9_]+) attribute in the (?<objectidtype>[a-z0-9_]+) entity. The [a-z0-9_]+ of the record is (?<objectid>[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+)", RegexOptions.IgnoreCase)
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

                                if (match.Groups["flsprivilegename"].Success)
                                {
                                    var privilege = ExtractFLSPrivilege(match, target, out var attributeName);
                                    var result = new FLSResults();
                                    result.Target = target;
                                    result.PrincipalTypeDisplayName = principalTypeDisplayName;
                                    result.TargetTypeDisplayName = targetTypeDisplayName;
                                    result.Privilege = privilege;
                                    result.AttributeName = attributeName;

                                    _targetReference = target as EntityReference;
                                    var entityName = _targetReference?.LogicalName ?? ((EntityMetadataCollection)target)[0].LogicalName;

                                    var metaQry = new RetrieveMetadataChangesRequest
                                    {
                                        Query = new EntityQueryExpression
                                        {
                                            Criteria = new MetadataFilterExpression
                                            {
                                                Conditions =
                                                {
                                                    new MetadataConditionExpression
                                                    {
                                                        PropertyName = nameof(EntityMetadata.LogicalName),
                                                        ConditionOperator = MetadataConditionOperator.Equals,
                                                        Value = entityName
                                                    }
                                                }
                                            },
                                            Properties = new MetadataPropertiesExpression
                                            {
                                                PropertyNames =
                                                {
                                                    nameof(EntityMetadata.Attributes),
                                                    nameof(EntityMetadata.ObjectTypeCode)
                                                }
                                            },
                                            AttributeQuery = new AttributeQueryExpression
                                            {
                                                Criteria = new MetadataFilterExpression
                                                {
                                                    Conditions =
                                                    {
                                                        new MetadataConditionExpression
                                                        {
                                                            PropertyName = nameof(AttributeMetadata.LogicalName),
                                                            ConditionOperator = MetadataConditionOperator.Equals,
                                                            Value = result.AttributeName
                                                        }
                                                    }
                                                },
                                                Properties = new MetadataPropertiesExpression
                                                {
                                                    PropertyNames =
                                                    {
                                                        nameof(AttributeMetadata.MetadataId)
                                                    }
                                                }
                                            }
                                        }
                                    };

                                    var metadata = (RetrieveMetadataChangesResponse)Service.Execute(metaQry);

                                    // Check if the problem should still exist
                                    try
                                    {
                                        var fieldAccess = (RetrievePrincipalAttributePrivilegesResponse)Service.Execute(new RetrievePrincipalAttributePrivilegesRequest
                                        {
                                            Principal = _principalReference
                                        });

                                        var permissions = fieldAccess.AttributePrivileges.SingleOrDefault(ap => ap.AttributeId == metadata.EntityMetadata[0].Attributes[0].MetadataId);
                                        
                                        if (permissions != null)
                                        {
                                            switch (result.Privilege)
                                            {
                                                case "Create":
                                                    result.HasAccess = permissions.CanCreate == 4;
                                                    break;

                                                case "Update":
                                                    result.HasAccess = permissions.CanUpdate == 4;
                                                    break;

                                                case "Read":
                                                    result.HasAccess = permissions.CanRead == 4;
                                                    break;
                                            }
                                        }
                                    }
                                    catch (FaultException<OrganizationServiceFault>)
                                    {
                                        // In case the service doesn't support the RetrievePrincipalAttributePrivileges message
                                    }

                                    var whoAmI = (WhoAmIResponse)Service.Execute(new WhoAmIRequest());
                                    if (_principalReference.LogicalName == "systemuser" && _principalReference.Id == whoAmI.UserId)
                                    {
                                        result.IsCurrentUser = true;
                                    }
                                    else
                                    {
                                        var resolutions = new List<Resolution>();

                                        // Find field security profiles that grant the required permission
                                        var sufficientProfileQry = new FetchExpression($@"
                                            <fetch xmlns:generator='MarkMpn.SQL4CDS'>
                                              <entity name='fieldsecurityprofile'>
                                                <attribute name='fieldsecurityprofileid' />
                                                <attribute name='name' />
                                                <link-entity name='fieldpermission' to='fieldsecurityprofileid' from='fieldsecurityprofileid' link-type='inner'>
                                                  <filter>
                                                    <condition attribute='entityname' operator='eq' value='{metadata.EntityMetadata[0].ObjectTypeCode}' />
                                                    <condition attribute='attributelogicalname' operator='eq' value='{result.AttributeName}' />
                                                    <condition attribute='can{result.Privilege.ToLower()}' operator='eq' value='4' />
                                                  </filter>
                                                </link-entity>
                                                <link-entity name='{_principalReference.LogicalName}profiles' to='fieldsecurityprofileid' from='fieldsecurityprofileid' alias='sup' link-type='outer'>
                                                  <filter>
                                                    <condition attribute='{_principalReference.LogicalName}id' operator='eq' value='{_principalReference.Id}' />
                                                  </filter>
                                                </link-entity>
                                                <filter>
                                                  <condition attribute='{_principalReference.LogicalName}profileid' entityname='sup' operator='null' />
                                                </filter>
                                              </entity>
                                            </fetch>");
                                        var sufficientProfiles = Service.RetrieveMultiple(sufficientProfileQry);

                                        foreach (var profile in sufficientProfiles.Entities)
                                        {
                                            var profileRef = profile.ToEntityReference();
                                            profileRef.Name = profile.GetAttributeValue<string>("name");

                                            var addRole = new AddFieldSecurityProfile
                                            {
                                                UserReference = _principalReference,
                                                ProfileReference = profileRef
                                            };

                                            resolutions.Add(addRole);
                                        }

                                        result.Resolutions = resolutions;
                                    }

                                    args.Result = result;
                                }
                                else
                                {
                                    var privilege = ExtractPrivilege(match, ref target, ref targetTypeDisplayName);
                                    var privilegeDepth = ExtractPrivilegeDepth(match, _principalReference, target);

                                    var result = new RoleResults();
                                    result.Target = target;
                                    result.PrincipalTypeDisplayName = principalTypeDisplayName;
                                    result.TargetTypeDisplayName = targetTypeDisplayName;
                                    result.Privilege = privilege;
                                    result.PrivilegeDepth = privilegeDepth;

                                    result.AccessRights = (AccessRights)privilege.GetAttributeValue<int>("accessright");

                                    _targetReference = target as EntityReference;
                                    var privilegeRef = privilege.ToEntityReference();
                                    privilegeRef.Name = privilege.GetAttributeValue<string>("name");

                                    // Check if the problem should still exist
                                    try
                                    {
                                        if (_targetReference != null && result.AccessRights != AccessRights.None)
                                        {
                                            var recordAccess = (RetrievePrincipalAccessResponse)Service.Execute(new RetrievePrincipalAccessRequest
                                            {
                                                Principal = _principalReference,
                                                Target = _targetReference
                                            });

                                            if ((recordAccess.AccessRights & result.AccessRights) == result.AccessRights)
                                                result.HasAccess = true;
                                        }
                                        else
                                        {
                                            var priv = (RetrieveUserPrivilegeByPrivilegeIdResponse)Service.Execute(new RetrieveUserPrivilegeByPrivilegeIdRequest
                                            {
                                                UserId = _principalReference.Id,
                                                PrivilegeId = privilege.Id
                                            });

                                            if (priv.RolePrivileges.Any())
                                                result.HasAccess = true;
                                        }
                                    }
                                    catch (FaultException<OrganizationServiceFault>)
                                    {
                                        // In case the service doesn't support the RetrieveUserPrivilegeByPrivilegeId message
                                    }

                                    // Check permission is available at the minimum calculated depth. If not, increase the depth
                                    // to the next available value.
                                    if (privilegeDepth == PrivilegeDepth.Basic && !privilege.GetAttributeValue<bool>("canbebasic"))
                                        privilegeDepth = PrivilegeDepth.Local;

                                    if (privilegeDepth == PrivilegeDepth.Local && !privilege.GetAttributeValue<bool>("canbelocal"))
                                        privilegeDepth = PrivilegeDepth.Deep;

                                    if (privilegeDepth == PrivilegeDepth.Deep && !privilege.GetAttributeValue<bool>("canbedeep"))
                                        privilegeDepth = PrivilegeDepth.Global;

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

                                    var whoAmI = (WhoAmIResponse)Service.Execute(new WhoAmIRequest());
                                    if (_principalReference.LogicalName == "systemuser" && _principalReference.Id == whoAmI.UserId)
                                    {
                                        result.IsCurrentUser = true;
                                    }
                                    else
                                    {
                                        var resolutions = new List<Resolution>();

                                        // Find roles that include the required permission and suggest to add them to the user
                                        var depthQuery = Math.Pow(2, (int)privilegeDepth);

                                        // Only suggest roles in the correct business unit
                                        var principal = Service.Retrieve(_principalReference.LogicalName, _principalReference.Id, new ColumnSet("businessunitid"));
                                        var businessUnitId = principal.GetAttributeValue<EntityReference>("businessunitid").Id;

                                        var sufficientRoleQry = new FetchExpression($@"
                                        <fetch xmlns:generator='MarkMpn.SQL4CDS'>
                                            <entity name='role'>
                                                <attribute name='name' />
                                                <link-entity name='role' from='roleid' to='parentrootroleid'>
                                                    <link-entity name='roleprivileges' to='roleid' from='roleid' alias='rp' link-type='inner'>
                                                        <filter>
                                                            <condition attribute='privilegeid' operator='eq' value='{privilege.Id}' />
                                                            <condition attribute='privilegedepthmask' operator='ge' value='{depthQuery}' />
                                                        </filter>
                                                    </link-entity>
                                                </link-entity>
                                                <link-entity name='{_principalReference.LogicalName}roles' to='roleid' from='roleid' alias='sur' link-type='outer'>
                                                    <filter>
                                                        <condition attribute='{_principalReference.LogicalName}id' operator='eq' value='{_principalReference.Id}' />
                                                    </filter>
                                                </link-entity>
                                                <filter>
                                                    <condition attribute='businessunitid' operator='eq' value='{businessUnitId}' />
                                                    <condition entityname='sur' attribute='roleid' operator='null' />
                                                </filter>
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
                                                <attribute name='parentrootroleid' />
                                                <link-entity name='role' from='roleid' to='parentrootroleid'>
                                                    <link-entity name='roleprivileges' to='roleid' from='roleid' alias='rp' link-type='outer'>
                                                        <attribute name='privilegedepthmask' />
                                                        <filter>
                                                            <condition attribute='privilegeid' operator='eq' value='{privilege.Id}' />
                                                        </filter>
                                                    </link-entity>
                                                </link-entity>
                                                <link-entity name='{_principalReference.LogicalName}roles' to='roleid' from='roleid' alias='sur' link-type='inner'>
                                                    <filter>
                                                        <condition attribute='{_principalReference.LogicalName}id' operator='eq' value='{_principalReference.Id}' />
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
                                            var roleRef = role.GetAttributeValue<EntityReference>("parentrootroleid");
                                            var existingDepth = role.GetAttributeValue<AliasedValue>("rp.privilegedepthmask");

                                            var editRole = new EditSecurityRole
                                            {
                                                RoleReference = roleRef,
                                                PrivilegeReference = privilegeRef,
                                                Depth = privilegeDepth,
                                                ExistingDepth = existingDepth == null ? (PrivilegeDepth?)null : (PrivilegeDepth)Math.Log((int)existingDepth.Value, 2)
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
                                                AccessRights = result.AccessRights
                                            };

                                            resolutions.Add(sharing);
                                        }

                                        result.Resolutions = resolutions;
                                    }

                                    args.Result = result;
                                }
                            }
                            catch (Exception ex)
                            {
                                args.Result = new Results { Exception = ex };
                            }
                        },
                        PostWorkCallBack = args =>
                        {
                            var result = (Results)args.Result;
                            var roleResult = result as RoleResults;
                            var flsResult = result as FLSResults;

                            if (result.Exception != null)
                            {
                                errorLabel.Text = result.Exception.Message;

                                // If this is an ObjectDoesNotExist error, it's likely that the error is from a different instance
                                unchecked
                                {
                                    if (result.Exception is FaultException<OrganizationServiceFault> fault && fault.Detail.ErrorCode == (int)0x80040217)
                                    {
                                        errorLabel.Text += "\r\n\r\nAre you connected to the same instance the error message came from?";
                                    }
                                }

                                noMatchPanel.Visible = false;
                                errorPanel.Visible = true;
                            }
                            else
                            {
                                retryPanel.Visible = result.HasAccess;

                                var userPrefix = $"The {result.PrincipalTypeDisplayName} ";
                                var userName = _principalReference.Name;
                                userLinkLabel.Text = userPrefix + userName;
                                userLinkLabel.LinkArea = new LinkArea(userPrefix.Length, userName.Length);

                                if (roleResult != null)
                                {
                                    if (roleResult.AccessRights == AccessRights.None)
                                        missingPrivilegeLinkLabel.Text = $"does not have {roleResult.Privilege.GetAttributeValue<string>("name")} permission";
                                    else
                                        missingPrivilegeLinkLabel.Text = $"does not have {roleResult.AccessRights.ToString().Replace("Access", "")} permission";
                                }
                                else
                                {
                                    missingPrivilegeLinkLabel.Text = $"does not have {flsResult.Privilege} permission";
                                }

                                if (result.Target == null)
                                {
                                    targetLinkLabel.Visible = false;
                                }
                                else
                                {
                                    var prefix = flsResult != null ? $"on the {flsResult.AttributeName} attribute of the {result.TargetTypeDisplayName} " : $"on the {result.TargetTypeDisplayName} ";
                                    var link = "";

                                    if (_targetReference != null)
                                        link = _targetReference.Name ?? "<Unnamed>";
                                    else
                                        prefix += String.Join(", ", ((EntityMetadataCollection)result.Target).Select(m => m.DisplayName.UserLocalizedLabel.Label));

                                    targetLinkLabel.Text = prefix + link;
                                    targetLinkLabel.LinkArea = new LinkArea(prefix.Length, link.Length);
                                    targetLinkLabel.Visible = true;
                                }

                                if (roleResult != null)
                                    requiredPrivilegeLabel.Text = $"To resolve this error, the user needs to be granted the {roleResult.Privilege.GetAttributeValue<string>("name")} privilege to {roleResult.PrivilegeDepth} depth";
                                else
                                    requiredPrivilegeLabel.Text = $"To resolve this error, the user needs to be granted the {flsResult.Privilege} privilege";

                                if (result.IsCurrentUser)
                                {
                                    errorLabel.Text = "Because you are the affected user you cannot use this tool to automatically resolve any security problems. Please ask a system administrator to run this tool on your behalf.";
                                    errorPanel.Visible = true;
                                }
                                else
                                {
                                    foreach (var resolution in result.Resolutions)
                                    {
                                        var lvi = resolutionsListView.Items.Add(resolution.ToString());
                                        lvi.ImageIndex = resolution.ImageIndex;
                                        lvi.Tag = resolution;
                                    }

                                    resolutionsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                                }

                                noMatchPanel.Visible = false;
                                recordPermissionsPanel.Visible = true;
                            }
                        }
                    });
                }
            }
        }

        private string ExtractFLSPrivilege(Match match, object target, out string attributeName)
        {
            attributeName = match.Groups["attributename"].Value;
            return match.Groups["flsprivilegename"].Value;
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
            {
                if (principal != null && match.Groups["businessunitid"].Success)
                    return DepthFromPrincipalAndBusinessUnit(principal, new EntityReference("businessunit", new Guid(match.Groups["businessunitid"].Value)));
                
                return PrivilegeDepth.Basic;
            }

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

            return DepthFromPrincipalAndBusinessUnit(principal, buRef);
        }

        private PrivilegeDepth DepthFromPrincipalAndBusinessUnit(EntityReference principal, EntityReference buRef)
        {
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

        private Entity ExtractPrivilege(Match match, ref object target, ref string targetTypeDisplayName)
        {
            if (match.Groups["privilegename"].Success)
            {
                var privQry = new QueryByAttribute("privilege");
                privQry.AddAttributeValue("name", match.Groups["privilegename"].Value);
                privQry.ColumnSet = new ColumnSet("name", "accessright", "canbebasic", "canbelocal", "canbedeep", "canbeglobal");
                var privilege = Service.RetrieveMultiple(privQry).Entities.SingleOrDefault();

                if (privilege == null)
                    throw new ApplicationException($"Could not find privilege \"{privQry.Values[0]}\"");

                if (target == null && targetTypeDisplayName == null)
                {
                    var privOtcQry = new QueryByAttribute("privilegeobjecttypecodes");
                    privOtcQry.AddAttributeValue("privilegeid", privilege.Id);
                    privOtcQry.ColumnSet = new ColumnSet("objecttypecode");
                    var privOtc = Service.RetrieveMultiple(privOtcQry).Entities.FirstOrDefault();

                    if (privOtc != null)
                    {
                        var logicalName = privOtc.GetAttributeValue<string>("objecttypecode");
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
                        target = entityResults.EntityMetadata;
                        targetTypeDisplayName = "entity";
                    }
                }

                return privilege;
            }

            if (match.Groups["accessrights"].Success)
            {
                string targetType;

                if (target is EntityReference e)
                    targetType = e.LogicalName;
                else if (target is EntityMetadataCollection m)
                    targetType = m[0].LogicalName;
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
                                nameof(EntityMetadata.PrimaryIdAttribute),
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
                var qry = new QueryByAttribute(targetReference.LogicalName);
                qry.AddAttributeValue(targetMetadata.PrimaryIdAttribute, targetReference.Id);
                qry.ColumnSet = new ColumnSet(targetMetadata.PrimaryNameAttribute);
                var results = Service.RetrieveMultiple(qry);

                if (results.Entities.Count == 1)
                {
                    var target = results.Entities[0];
                    targetReference.Name = target.GetAttributeValue<string>(targetMetadata.PrimaryNameAttribute);
                    targetTypeDisplayName = targetMetadata.DisplayName.UserLocalizedLabel.Label;
                    return targetReference;
                }
                else
                {
                    targetTypeDisplayName = "entity";
                    return targetEntityResults.EntityMetadata;
                }
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
                var metadata = entityResults.EntityMetadata;

                targetTypeDisplayName = "entity";
                return metadata;
            }

            targetTypeDisplayName = null;
            return null;
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

        class AddFieldSecurityProfile : Resolution
        {
            public override int ImageIndex => 1;

            public EntityReference UserReference { get; set; }

            public EntityReference ProfileReference { get; set; }

            public override void Execute(IOrganizationService org)
            {
                org.Associate(
                    UserReference.LogicalName,
                    UserReference.Id,
                    new Relationship($"{UserReference.LogicalName}profiles_association"),
                    new EntityReferenceCollection { ProfileReference });
            }

            public override string ToString()
            {
                return $"Add the {ProfileReference.Name} field security profile to {UserReference.Name}";
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
                PostWorkCallBack = result =>
                {
                    if (result.Error != null)
                        MessageBox.Show(result.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        ParseError();
                }
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

        string IPayPalPlugin.DonationDescription => "Security Debugger Donation";

        string IPayPalPlugin.EmailAccount => "donate@markcarrington.dev";
    }
}
