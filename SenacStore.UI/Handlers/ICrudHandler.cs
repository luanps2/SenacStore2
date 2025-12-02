// Arquivo: SenacStore.UI.Handlers.ICrudHandler.cs
// Propósito: Interface que define as operações CRUD que os Handlers da UI devem implementar.
// Cada handler adapta um repositório/entidade ao ucCrudPadrao (listar, criar, editar, deletar).

using System;                // Tipos básicos (por exemplo Guid)
using System.Collections.Generic; // Coleções genéricas (não usadas diretamente aqui, mas comum em handlers)

namespace SenacStore.UI.Handlers
{
    // Interface pública para handlers CRUD usados pela camada de UI
    public interface ICrudHandler
    {
        // Propriedade que expõe o título (nome da entidade) para exibição no cabeçalho da tela
        string Titulo { get; }

        // Método que retorna todos os registros (usado como DataSource para o DataGridView).
        // Tipo object permite que o handler projete a lista (por exemplo, lista de objetos anônimos).
        object ObterTodos();

        // Abre a tela/controle para criação de um novo registro.
        void Criar();

        // Abre a tela/controle para edição de um registro existente identificado por id.
        void Editar(Guid id);

        // Remove o registro identificado por id do repositório (persistência).
        void Deletar(Guid id);
    }
}
