<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fills_Entry.aspx.cs" Inherits="Flying_Hour_Fills_Entry" %>

<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fill Entry</title>

    <link href="../assets/stylesheets/bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="../assets/stylesheets/light-theme.css" rel="stylesheet" />
    <link href="../assets/stylesheets/theme-colors.css" rel="stylesheet" />
    <link href="../assets/stylesheets/demo.css" rel="stylesheet" />
    <link href="../font-awesome/css/font-awesome.min.css" rel="stylesheet" />

    <style>
        .gridview-style {
            width: 100% !important;
            border-collapse: collapse;
        }

            .gridview-style th {
                background-color: #243b55 !important;
                color: #ffffff !important;
                text-align: center !important;
                vertical-align: middle !important;
                padding: 12px 10px !important;
                font-size: 14px !important;
                font-weight: bold !important;
                white-space: nowrap;
                border: 1px solid #1b2f45 !important;
            }

            .gridview-style td {
                text-align: center !important;
                vertical-align: middle !important;
                padding: 10px !important;
                border: 1px solid #ddd !important;
                white-space: nowrap;
            }

            .gridview-style tr:nth-child(even) td {
                background-color: #f8f9fa !important;
            }

            .gridview-style tr:hover td {
                background-color: #eaf2ff !important;
            }

        .grid-scroll {
            width: 100%;
            overflow-x: auto;
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
                                        <i class="fa fa-bars"></i>
                                        Fill Entry
                                    </h4>
                                </div>
                            </div>
                            <div class="box-content">
                                <div class="row" style="padding-left: 10px; padding-right: 10px;">

                                    <div class="grid-scroll">
                                        <asp:GridView ID="gridview" runat="server" AutoGenerateColumns="false" CssClass="gridview-style" Style="width: 100%;">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %> </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Student Name" DataField="StudentName" />
                                                <asp:BoundField HeaderText="Mobile No" DataField="MobileNo" />
                                                <asp:BoundField HeaderText="Email" DataField="Email" />
                                                <asp:BoundField HeaderText="Gender" DataField="Gender" />
                                                <asp:BoundField HeaderText="Course Name" DataField="CourseName" />
                                                <asp:TemplateField HeaderText="Marksheet">
                                                    <ItemTemplate><a href='<%# "?ViewFile=Marksheet&ID=" + Eval("ID") %>' target="_blank">View Marksheet</a></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Sign">
                                                    <ItemTemplate><a href='<%# "?ViewFile=Sign&ID=" + Eval("ID") %>' target="_blank">View Sign</a></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Entry Date" DataField="EntryDate" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkFillData" runat="server" Text="Verify" CssClass="btn btn-primary btn-sm" CommandArgument='<%# Eval("ID") %>' OnClick="lnkFillData_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                </section>
            </ContentTemplate>

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
