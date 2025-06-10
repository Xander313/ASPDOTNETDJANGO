<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mantenimieto.aspx.cs" Inherits="ejercicio5ASPDOTNET.Mantenimieto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
    <div class="row justify-content-center">
        <div class="col-auto d-flex justify-content-center align-items-center">
            <h2>MANTENIMIENTOS REGISTRADOS</h2>
        </div>
    </div>
    <hr />
</div>

<br />
<div class="container">
    <div class="row justify-content-center">
        <div class="col-auto d-flex justify-content-center align-items-center">
            <asp:Button runat="server" ID="BtnCreateMantenimiento" CssClass="btn btn-success form-control-sm" Text="Registrar Mantenimiento" OnClick="BtnCreateMantenimiento_Click"/>
        </div>
    </div>
</div>

<br />
<div class="container">
    <div class="row justify-content-center">
        <div class="col-lg-12">
            <asp:GridView runat="server" ID="DGVMantenimientos" CssClass="table table-borderless table-hover text-center" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="MantenimientoID" HeaderText="ID" ItemStyle-CssClass="text-center" />
                    <asp:BoundField DataField="ImpresoraID" HeaderText="Impresora ID" ItemStyle-CssClass="text-center" />
                    <asp:BoundField DataField="FechaMantenimiento" HeaderText="Fecha" ItemStyle-CssClass="text-center" />
                    <asp:BoundField DataField="Tecnico" HeaderText="Técnico Responsable" ItemStyle-CssClass="text-center" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ItemStyle-CssClass="text-center" />
                    <asp:TemplateField HeaderText="Informe">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "InformeHandler.ashx?id=" + Eval("MantenimientoID") %>' Text="📄 Descargar PDF" CssClass="btn btn-sm btn-primary"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Opciones">
                        <ItemTemplate>
                            <asp:Button runat="server" Text="🔄 Editar" CssClass="btn form-control-sm btn-warning" ID="BtnUpdateMantenimiento" OnClick="BtnUpdateMantenimiento_Click"/>
                            <asp:Button runat="server" Text="❌ Eliminar"
                                CssClass="btn form-control-sm btn-danger"
                                ID="BtnDeleteMantenimiento"
                                OnClick="BtnDeleteMantenimiento_Click"
                                OnClientClick='<%# "return confirmarEliminacionMantenimiento(event, " + Eval("MantenimientoID") + ");" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

<script>
    function confirmarEliminacionMantenimiento(event, idMantenimiento) {
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
                window.location.href = 'Default.aspx?deleteMantenimientoId=' + idMantenimiento; 
            }
        });
    }
</script>




</asp:Content>
