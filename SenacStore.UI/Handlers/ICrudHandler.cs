using System;
using System.Collections.Generic;

namespace SenacStore.UI.Handlers
{
    public interface ICrudHandler
    {
        string Titulo { get; }                                // Nome da entidade
        object ObterTodos();                                  // Para o DataGridView
        void Criar();                                         // Abrir formulário de criação
        void Editar(Guid id);                                 // Abrir formulário de edição
        void Deletar(Guid id);                                // Deletar registro
    }
}
