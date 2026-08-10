<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyFlyingHours.aspx.cs" Inherits="Flying_Hour_DailyFlyingHours" %>

<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Daily Flying Hours</title>

    <link href="../assets/stylesheets/bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="../assets/stylesheets/light-theme.css" rel="stylesheet" />
    <link href="../assets/stylesheets/theme-colors.css" rel="stylesheet" />
    <link href="../assets/stylesheets/demo.css" rel="stylesheet" />
    <link href="../font-awesome/css/font-awesome.min.css" rel="stylesheet" />

    <style>
        .form-group {
            margin-bottom: 15px;
        }

        .control-label {
            font-weight: bold;
        }

        .mandatoryfields {
            color: red;
        }

        .btn-space {
            margin-right: 10px;
        }

        .table > thead > tr > th {
            background: #337ab7;
            color: #fff;
            text-align: center;
            vertical-align: middle;
        }

        .table > tbody > tr > td {
            vertical-align: middle;
        }

        .form-control {
            height: 38px;
        }

        .btn {
            min-width: 110px;
        }

        .box-content {
            padding: 20px;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"
            EnableViewState="true"
            EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="~/EducationService.asmx" />
            </Services>
        </asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">

            <ContentTemplate>

                <section>

                    <div class="col-sm-12 col-lg-12">

                        <div class="box">

                            <div class="box-header blue-background">
                                <div class="title">
                                    <h4>
                                        <i class="fa fa-upload"></i>
                                        Daily Flying Hours
                                    </h4>
                                </div>
                            </div>

                            <div class="box-content">

                                <div id="ExceptionMsg"
                                    runat="server"
                                    style="display: none; color: red;">
                                </div>
                                <div class="row" style="padding: 20px;">

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label">Download Excel Template</label><br />
                                            <asp:Button ID="btnDownloadExcel"
                                                runat="server"
                                                Text="Download Template"
                                                CssClass="btn btn-success"
                                                OnClick="btnDownloadExcel_Click" />
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label">
                                                Select File
               
                                                <span class="mandatoryfields">*</span>
                                            </label>

                                            <asp:FileUpload
                                                ID="fuExcel"
                                                runat="server"
                                                CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="col-md-6 text-center" style="padding-top: 20px;">
                                        <asp:Button  ID="btnUploadExcel"  runat="server" Text="Import" CssClass="btn btn-primary" OnClick="btnUploadExcel_Click" />
                                        &nbsp;
                                        <%--<asp:Button  ID="btnre"  runat="server"  Text="Reset"  CssClass="btn btn-default" />--%>
                                        <asp:Button  ID="btnSave"  runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />

                                    </div>
                                </div>
                               
                                <div class="table-responsive">
                                    <asp:GridView ID="gridview"
                                        runat="server"
                                        AutoGenerateColumns="False"
                                        CssClass="table table-bordered table-striped table-hover">

                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Student Reg." DataField="StudentReg" />
                                            <asp:BoundField HeaderText="Instructor Name" DataField="InstructorName" />
                                            <asp:BoundField HeaderText="Flying Date" DataField="FlyingDate"/>
                                            <asp:BoundField HeaderText="Flying Hours" DataField="FlyingHours" />
                                            <asp:BoundField HeaderText="Status" DataField="Status" />
                                            <asp:BoundField HeaderText="Uploaded By" DataField="UploadedBy" />
                                            <asp:BoundField HeaderText="Uploaded Date" DataField="UploadedDate"/>

                                        </Columns>

                                        <HeaderStyle CssClass="bg-primary" />
                                        <EmptyDataTemplate>
                                            <div class="text-center text-danger" style="padding: 15px;">
                                                No Record Found.
           
                                            </div>
                                        </EmptyDataTemplate>

                                    </asp:GridView>
                                </div>


                            </div>

                        </div>
                    </div>

                </section>

            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnDownloadExcel" />
                <asp:PostBackTrigger ControlID="btnUploadExcel" />
            </Triggers>

        </asp:UpdatePanel>

        <asp:UpdateProgress
            ID="UpdateProgress3"
            runat="server"
            AssociatedUpdatePanelID="UpdatePanel1"
            DisplayAfter="0">

            <ProgressTemplate>

                <div class="ProgressMsg">

                    <img src="../images/wait.gif"
                        alt="Wait"
                        style="margin-top: 300px;" />

                    <br />
                    Please wait...

                </div>

            </ProgressTemplate>

        </asp:UpdateProgress>

        <cc2:UpdateProgressOverlayExtender
            ID="UpdateProgressOverlayExtender3"
            runat="server"
            TargetControlID="UpdateProgress3"
            CssClass="updateProgress"
            OverlayType="Browser" />

    </form>

</body>
</html>
