// Arquivo: SenacStore.UI\InterfacesUI\IRefreshable.cs
// Finalidade: define um contrato para controles que precisam recarregar o grid/lista ao serem exibidos ou após operações.

public interface IRefreshable
{
    // Método que deve atualizar/recarregar os dados exibidos (ex.: DataGridView).
    void RefreshGrid();
}
