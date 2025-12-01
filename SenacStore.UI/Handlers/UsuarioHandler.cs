using System;
using System.Linq;
using SenacStore.Application;
using SenacStore.UI.Navigation;
using SenacStore.UI.UserControls;

namespace SenacStore.UI.Handlers
{
    public class UsuarioHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ITipoUsuarioRepository _tipoRepo;

        public string Titulo => "Usuários";

        public UsuarioHandler(ICrudNavigator nav, IUsuarioRepository usuarioRepo, ITipoUsuarioRepository tipoRepo)
        {
            _nav = nav;
            _usuarioRepo = usuarioRepo;
            _tipoRepo = tipoRepo;
        }

        public object ObterTodos()
        {
            var tipos = _tipoRepo.ObterTodos().ToDictionary(t => t.Id, t => t.Nome);

            // Inclui FotoUrl na projeção para permitir exibição no grid
            return _usuarioRepo.ObterTodos()
                .Select(u => new {
                    u.Id,
                    u.Nome,
                    u.Email,
                    FotoUrl = u.FotoUrl, // agora disponível
                    Tipo = tipos.ContainsKey(u.TipoUsuarioId) ? tipos[u.TipoUsuarioId] : "Desconhecido"
                })
                .ToList();
        }

        public void Criar() =>
            _nav.Abrir(new ucUsuarios(_nav, _usuarioRepo, _tipoRepo));

        public void Editar(Guid id) =>
            _nav.Abrir(new ucUsuarios(_nav, _usuarioRepo, _tipoRepo, id));

        public void Deletar(Guid id) =>
            _usuarioRepo.Deletar(id);
    }
}
