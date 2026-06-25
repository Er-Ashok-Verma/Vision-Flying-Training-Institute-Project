<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Instructor_Master.aspx.cs" Inherits="Fee_Instructor_Master" %>


<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fee Head Master</title>
    <link href="../assets/stylesheets/bootstrap/bootstrap.css" media="all" rel="stylesheet" type="text/css" />   
    <link href="../assets/stylesheets/light-theme.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/theme-colors.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/demo.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../font-awesome/css/font-awesome.css" rel="stylesheet" />
    <script src="../js/JScript.js" type="text/javascript"></script>
    <script src="../Scripts/Global.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <section id=''>
        <div class=''>
          <div id='Div1'>
                <div class='col-sm-12 col-lg-12'>
                  <div class='box'>
                    <div class='box-header blue-background'>
                      <div class='title'>
                       <h4>
                       <i class="fa fa-bars"></i> Instructor Master
                           </h4>                      
                        </div>                     
                      </div>
                   
                         
                      <div class='col-sm-4 col-lg-4'>
                        <div class='box-content'>
                        <%--  <div class='form-group' runat="server" id="divInst">
                             <label for='inputText'>Institute</label>
                                <span class="mandatoryfields">*</span> 
                                  <asp:DropDownList ID="ddlInstitute" runat="server" class="form-control" AutoPostBack="true"
                                       OnSelectedIndexChanged="ddlInstitute_SelectedIndexChanged">
                                  </asp:DropDownList>                        
                            </div>--%>

                            <div class='form-group '>
                              <label for='inputText'>  Name</label>
                                 <span class="mandatoryfields">*</span> 
                              <asp:TextBox ID="txtname" runat="server"  class="form-control" MaxLength ="100"  ></asp:TextBox>                               
                             </div>

                             <div class='form-group '>
                              <label for='inputText'>Email</label>
                                  <span class="mandatoryfields">*</span> 
                                 <asp:TextBox ID="txtemail" runat="server"   class="form-control"  ></asp:TextBox>                 
                            </div>

                           
                            <div class='form-group '>
                              <label for='inputText'>Mobile No.</label>
                                 <span class="mandatoryfields">*</span> 
                                 <asp:TextBox ID="txtmobileNo" runat="server" onKeyPress="return isNumberKey(event);" class="form-control" MaxLength="10"></asp:TextBox> 
                            </div>
                             
                          <div class='form-actions form-actions-padding-sm form-actions-padding-md form-actions-padding-lg' style='margin-bottom: 0;'>  
                               <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"   Text="Submit" CssClass="btn btn-primary" />     
                               <asp:Button ID="btnReset" runat="server" CausesValidation="False"  Text="Reset" CssClass="btn btn-inverse" />                              
                               <asp:Button ID="btnDel" runat="server"     OnClientClick="return confirm('Are you sure, want to delete this fee head.?.');"
                                     Text="Delete" CssClass="btn btn-danger"  />              
                         </div>                 
                        </div>                
                      </div>                 

                    <div class='col-sm-8 col-lg-8'>
                    <div class='box-content overflow'>
                        <div class="DivVerticalScroll">                
                          <asp:GridView ID="gridview" runat="server" 
                            AutoGenerateColumns="False" DataKeyNames="ID" Caption=""
                           class="scrollable-area table table-bordered table-striped" >
                            <Columns>
                                 <asp:TemplateField HeaderText="Sr.No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Name"/>
                                <asp:BoundField DataField="MobileNo" HeaderText="MobileNo"></asp:BoundField>
                                <asp:BoundField DataField="Eamil" HeaderText="Email"></asp:BoundField>
                                
                                <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" ToolTip="Edit"
                                                    CommandName="EditRow"  
                                                    CssClass="text-primary">
                                                <i class="fas fa-edit" ></i>
                                                </asp:LinkButton>

                                                 <asp:LinkButton ID="lnkbtndelete" runat="server" ForeColor="Red" CausesValidation="False" 
                                            CommandName="Delete" Style="margin-left: 12px;"
                                             Text='<i class="fas fa-trash-alt"></i>' ToolTip="Delete" 
                                            OnClientClick="return confirm('Are you sure to delete this Data?');">
                                                     </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                            </Columns>
                         <EmptyDataRowStyle />
                         <EmptyDataTemplate>No fee head found to display.</EmptyDataTemplate>
                        </asp:GridView>
                       </div>                             
                        <input type="hidden" id="hdnCompanyName" runat="server" /> 
                        <input type="hidden" id="hdnFeeHeadLedgerName" runat="server" />
                        <input type="hidden" id="hdnFeeHeadLedgerAdditionalName" runat="server" />
                        <input type="hidden" id="hdnFeeHeadLedgerParentName" runat="server" />         
                        <input type="hidden" id="hdnInstituteID" runat="server" />
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              </div>            
              </section>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
            DisplayAfter="0">
            <ProgressTemplate>
                <div class="ProgressMsg">
                    <img src="../images/wait.gif" alt="Wait" />Please wait..
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <cc2:UpdateProgressOverlayExtender ID="UpdateProgressOverlayExtender3" runat="server"
            CssClass="updateProgress" TargetControlID="UpdateProgress3" OverlayType="Browser" />
    </form>
</body>
</html>
