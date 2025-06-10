<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarMantenimiento.aspx.cs" Inherits="ejercicio5ASPDOTNET.AgregarMantenimiento" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="mx-auto text-center" style="width: 100%; max-width: 100%;">
        <asp:Label ID="lblTitulo" runat="server" style="font-size:30px;" class="text-center" Text="REGISTRAR MANTENIMIENTO"></asp:Label>
    </div>
    <hr />

    <!-- Impresoras disponibles -->
<div class="container container-red mx-auto w-100">
    <div class="d-flex flex-row align-items-center justify-content-center">
        <div class="d-flex align-items-center">
            <p class="mb-0 me-3">Seleccionar impresora:</p>
            <asp:DropDownList runat="server" ID="ddlImpresoras" CssClass="form-control" />
        </div>
    </div>
</div>



    <!-- Datos del mantenimiento -->
    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Fecha de Mantenimiento:</p>
                <asp:TextBox runat="server" CssClass="form-control" ID="txbFechaMantenimiento" TextMode="Date" Style="width: 400px;" />
            </div>
        </div>
    </div>

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Técnico Responsable:</p>
                <asp:TextBox runat="server" CssClass="form-control" ID="txbTecnico" Style="width: 400px;" />
            </div>
        </div>
    </div>

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Descripción:</p>
                <asp:TextBox runat="server" CssClass="form-control" ID="txbDescripcion" Style="width: 400px;" TextMode="MultiLine" Rows="4"/>
            </div>
        </div>
    </div>

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Informe en PDF:</p>
                <asp:FileUpload runat="server" ID="FileUploadPDF" CssClass="form-control"/>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col d-flex justify-content-center">
                <asp:Button runat="server" ID="BtnCrearMantenimiento" CssClass="btn btn-success form-control-sm d-inline-block" Text="Registrar Mantenimiento" Visible="true" OnClick="BtnCrearMantenimiento_Click" />
            </div>
        </div>
    </div>
</asp:Content>
