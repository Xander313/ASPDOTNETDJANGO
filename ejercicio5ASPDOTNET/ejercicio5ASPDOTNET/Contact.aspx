<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="ejercicio5ASPDOTNET.Contact" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="mx-auto text-center" style="width: 100%; max-width: 100%;">
        <asp:Label ID="lblTitulo" runat="server" style="font-size:30px;" class="text-center" Text="REGISTRAR IMPRESORA"></asp:Label>
    </div>
    <hr />

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center" style="min-height: 50px; margin-bottom: 10px;">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Marca:</p>
                <asp:TextBox runat="server" CssClass="form-control" ID="txbMarca" Style="width: 400px;" />
            </div>
        </div>
    </div>

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center" style="min-height: 50px; margin-bottom: 10px;">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Modelo:</p>
                <asp:TextBox runat="server" CssClass="form-control" ID="txbModelo" Style="width: 400px;" />
            </div>
        </div>
    </div>

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center" style="min-height: 50px; margin-bottom: 10px;">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Ubicación:</p>
                <asp:TextBox runat="server" CssClass="form-control" ID="txbUbicacion" Style="width: 400px;" />
            </div>
        </div>
    </div>

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center" style="min-height: 50px; margin-bottom: 10px;">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Fecha de Adquisición:</p>
                <asp:TextBox runat="server" CssClass="form-control" ID="txbFechaAdquisicion" TextMode="Date" Style="width: 400px;" />
            </div>
        </div>
    </div>

    <div class="container container-red mx-auto w-100">
        <div class="d-flex flex-row align-items-center justify-content-center" style="min-height: 50px; margin-bottom: 10px;">
            <div class="d-flex align-items-center">
                <p class="mb-0 me-3">Imagen:</p>
                <asp:FileUpload runat="server" ID="fuImagen" CssClass="form-control" />
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col d-flex justify-content-center">
                <asp:Button runat="server" ID="BtnCrearImp" CssClass="btn btn-success form-control-sm d-inline-block" Text="Crear Impresora" Visible="true" OnClick="BtnCrearImp_Click"/>
            </div>
        </div>
    </div>
    


</asp:Content>
