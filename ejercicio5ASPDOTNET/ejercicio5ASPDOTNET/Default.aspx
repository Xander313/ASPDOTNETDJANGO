<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ejercicio5ASPDOTNET._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div>
            <asp:Button runat="server" ID="BtnRegresar" CssClass="btn btn-info form-control-sm d-inline-block" Text="Regresar"/>
        </div>

        <div class="container">
            <div class="row justify-content-center">
                <div class="col-auto d-flex justify-content-center align-items-center">
                    <h2>IMPRESORAS REGISTRADAS</h2>
                </div>
            </div>
            <hr />
        </div>

        <br />
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-auto d-flex justify-content-center align-items-center">
                    <asp:Button runat="server" ID="BtnCreate" CssClass="btn btn-success form-control-sm" Text="Agregar nueva impresora" OnClick="BtnCreate_Click"/>
                </div>
            </div>
        </div>

        <br />

        <div class="container">

            <div class="row justify-content-center">
                <div class="col-lg-12">
                    <asp:GridView runat="server" ID="DGVImpresoras" CssClass="table table-borderless table-hover text-center" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="ImpresoraID" HeaderText="ID" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Marca" HeaderText="Marca" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="FechaAdquisicion" HeaderText="Fecha de Adquisición" ItemStyle-CssClass="text-center" />
                            <asp:TemplateField HeaderText="Imagen">
                                <ItemTemplate>
                                    <asp:Image runat="server" ImageUrl='<%# Eval("ImagenBase64") %>' CssClass="img-thumbnail" Width="100" Height="100"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Opciones">
                                <ItemTemplate>
                                    <asp:Button runat="server" Text="🔄 Editar" CssClass="btn form-control-sm btn-warning" ID="BtnUpdate" OnClick="BtnUpdate_Click"/>
<asp:Button runat="server" Text="❌ Eliminar"
    CssClass="btn form-control-sm btn-danger"
    ID="BtnDelete"
    OnClick="BtnDelete_Click"
    OnClientClick='<%# "return confirmarEliminacion(event, " + Eval("ImpresoraID") + ");" %>' />


                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>



<script>
    function confirmarEliminacion(event, idImpresora) {
        event.preventDefault(); 

        Swal.fire({
            title: '¿Estás seguro?',
            text: 'Esta acción no se puede revertir.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = 'Default.aspx?deleteId=' + idImpresora; // 🔥 Redirigir con el ID
            }
        });
    }
</script>



</asp:Content>