// Especificação OpenAPI da API. A página em /docs é montada a partir deste objeto.
// Ao criar ou mudar uma rota, atualize também este arquivo.

// ---------- Atalhos para não repetir o mesmo trecho em todas as rotas ----------

// Corpo JSON que segue um schema de components/schemas
const json = nome => ({ 'application/json': { schema: { $ref: `#/components/schemas/${nome}` } } })
const listaJson = nome => ({ 'application/json': { schema: { type: 'array', items: { $ref: `#/components/schemas/${nome}` } } } })

// Resposta de erro no formato { mensagem }, com um exemplo de texto
const erro = (descricao, mensagem) => ({
  description: descricao,
  content: { 'application/json': { schema: { $ref: '#/components/schemas/Erro' }, example: { mensagem } } }
})

const SEM_TOKEN = erro('Token ausente, inválido ou expirado', 'Sua sessão expirou. Faça login novamente.')
const ID = { $ref: '#/components/parameters/Id' }
const NOME = { $ref: '#/components/parameters/Nome' }

// ---------- Especificação ----------

export const swaggerSpec = {
  openapi: '3.0.3',
  info: {
    title: 'API SenacStore',
    version: '1.0.0',
    description:
      'Como testar as rotas protegidas:\n\n' +
      '1. Abra **POST /auth/login**, clique em **Try it out** e em **Execute** (o exemplo já vem com o admin).\n' +
      '2. Copie o valor de `token` da resposta.\n' +
      '3. Clique em **Authorize**, cole o token (sem a palavra Bearer) e confirme.'
  },

  // URL relativa: a página funciona igual em localhost e no Azure
  servers: [{ url: '/' }],

  // Toda rota exige token, exceto as que declaram security: []
  security: [{ bearerAuth: [] }],

  tags: [
    { name: 'Autenticação' },
    { name: 'Tipos de usuário' },
    { name: 'Categorias' },
    { name: 'Produtos' },
    { name: 'Usuários' }
  ],

  components: {
    securitySchemes: {
      bearerAuth: { type: 'http', scheme: 'bearer', bearerFormat: 'JWT' }
    },

    parameters: {
      Id: {
        name: 'id', in: 'path', required: true,
        description: 'ID do registro (GUID)',
        schema: { type: 'string', format: 'uuid' }
      },
      Nome: {
        name: 'nome', in: 'query', required: false,
        description: 'Traz só os registros cujo nome contém este termo',
        schema: { type: 'string' }
      }
    },

    schemas: {
      Erro: {
        type: 'object',
        properties: { mensagem: { type: 'string' } }
      },

      Login: {
        type: 'object',
        required: ['email', 'senha'],
        properties: {
          email: { type: 'string', example: 'admin@marketplace.com' },
          senha: { type: 'string', example: 'adminpass' }
        }
      },
      LoginResposta: {
        type: 'object',
        properties: {
          token: { type: 'string', example: 'eyJhbGciOiJIUzI1NiIs...' },
          usuario: { $ref: '#/components/schemas/Usuario' }
        }
      },

      TipoUsuario: {
        type: 'object',
        properties: {
          id: { type: 'string', format: 'uuid', example: '11111111-1111-1111-1111-111111111111' },
          nome: { type: 'string', example: 'Administrador' }
        }
      },
      TipoUsuarioEntrada: {
        type: 'object',
        required: ['nome'],
        properties: { nome: { type: 'string', maxLength: 100, example: 'Gerente' } }
      },

      Categoria: {
        type: 'object',
        properties: {
          id: { type: 'string', format: 'uuid', example: 'aaaaaaaa-0000-0000-0000-000000000002' },
          nome: { type: 'string', example: 'Informática' }
        }
      },
      CategoriaEntrada: {
        type: 'object',
        required: ['nome'],
        properties: { nome: { type: 'string', maxLength: 120, example: 'Esportes' } }
      },

      Produto: {
        type: 'object',
        properties: {
          id: { type: 'string', format: 'uuid', example: 'bbbbbbbb-0000-0000-0000-000000000004' },
          nome: { type: 'string', example: 'SSD NVMe 1TB' },
          preco: { type: 'number', example: 429.9 },
          categoriaId: { type: 'string', format: 'uuid', example: 'aaaaaaaa-0000-0000-0000-000000000002' }
        }
      },
      ProdutoEntrada: {
        type: 'object',
        required: ['nome', 'preco', 'categoriaId'],
        properties: {
          nome: { type: 'string', maxLength: 150, example: 'SSD NVMe 2TB' },
          preco: { type: 'number', minimum: 0, maximum: 99999999.99, example: 799.9 },
          categoriaId: { type: 'string', format: 'uuid', example: 'aaaaaaaa-0000-0000-0000-000000000002' }
        }
      },

      // A senha nunca aparece nas respostas
      Usuario: {
        type: 'object',
        properties: {
          id: { type: 'string', format: 'uuid', example: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' },
          nome: { type: 'string', example: 'Admin Plataforma' },
          email: { type: 'string', example: 'admin@marketplace.com' },
          tipoUsuarioId: { type: 'string', format: 'uuid', example: '11111111-1111-1111-1111-111111111111' }
        }
      },
      UsuarioCriar: {
        type: 'object',
        required: ['nome', 'email', 'senha', 'tipoUsuarioId'],
        properties: {
          nome: { type: 'string', maxLength: 150, example: 'Novo Comprador' },
          email: { type: 'string', maxLength: 150, example: 'novo@exemplo.com' },
          senha: { type: 'string', example: 'senha123' },
          tipoUsuarioId: { type: 'string', format: 'uuid', example: '33333333-3333-3333-3333-333333333333' }
        }
      },
      UsuarioAtualizar: {
        type: 'object',
        required: ['nome', 'email', 'tipoUsuarioId'],
        properties: {
          nome: { type: 'string', maxLength: 150, example: 'Cliente Teste' },
          email: { type: 'string', maxLength: 150, example: 'cliente@exemplo.com' },
          senha: { type: 'string', description: 'Opcional. Ausente ou vazia mantém a senha atual.', example: '' },
          tipoUsuarioId: { type: 'string', format: 'uuid', example: '33333333-3333-3333-3333-333333333333' }
        }
      }
    }
  },

  paths: {

    // ---------- Autenticação ----------

    '/auth/login': {
      post: {
        tags: ['Autenticação'],
        summary: 'Faz login e devolve o token',
        security: [],
        requestBody: { required: true, content: json('Login') },
        responses: {
          200: { description: 'Login feito', content: json('LoginResposta') },
          400: erro('E-mail ou senha ausentes', 'Informe e-mail e senha.'),
          401: erro('E-mail ou senha errados', 'E-mail ou senha inválidos.')
        }
      }
    },

    // ---------- Tipos de usuário ----------

    '/tipos-usuario': {
      get: {
        tags: ['Tipos de usuário'],
        summary: 'Lista os tipos de usuário (pública)',
        security: [],
        parameters: [NOME],
        responses: {
          200: { description: 'Lista de tipos', content: listaJson('TipoUsuario') }
        }
      },
      post: {
        tags: ['Tipos de usuário'],
        summary: 'Cria um tipo de usuário',
        requestBody: { required: true, content: json('TipoUsuarioEntrada') },
        responses: {
          201: { description: 'Tipo criado', content: json('TipoUsuario') },
          400: erro('Nome inválido', 'Informe o nome do tipo de usuário (até 100 caracteres).'),
          401: SEM_TOKEN
        }
      }
    },

    '/tipos-usuario/{id}': {
      parameters: [ID],
      get: {
        tags: ['Tipos de usuário'],
        summary: 'Busca um tipo de usuário',
        responses: {
          200: { description: 'Tipo encontrado', content: json('TipoUsuario') },
          401: SEM_TOKEN,
          404: erro('Tipo não encontrado', 'Tipo de usuário não encontrado.')
        }
      },
      put: {
        tags: ['Tipos de usuário'],
        summary: 'Atualiza um tipo de usuário',
        requestBody: { required: true, content: json('TipoUsuarioEntrada') },
        responses: {
          200: { description: 'Tipo atualizado', content: json('TipoUsuario') },
          400: erro('Nome inválido', 'Informe o nome do tipo de usuário (até 100 caracteres).'),
          401: SEM_TOKEN,
          404: erro('Tipo não encontrado', 'Tipo de usuário não encontrado.')
        }
      },
      delete: {
        tags: ['Tipos de usuário'],
        summary: 'Exclui um tipo de usuário',
        responses: {
          204: { description: 'Tipo excluído (sem corpo)' },
          401: SEM_TOKEN,
          404: erro('Tipo não encontrado', 'Tipo de usuário não encontrado.'),
          409: erro('Tipo ainda tem usuários', 'Este tipo ainda tem usuários e não pode ser excluído.')
        }
      }
    },

    // ---------- Categorias ----------

    '/categorias': {
      get: {
        tags: ['Categorias'],
        summary: 'Lista as categorias',
        parameters: [NOME],
        responses: {
          200: { description: 'Lista de categorias', content: listaJson('Categoria') },
          401: SEM_TOKEN
        }
      },
      post: {
        tags: ['Categorias'],
        summary: 'Cria uma categoria',
        requestBody: { required: true, content: json('CategoriaEntrada') },
        responses: {
          201: { description: 'Categoria criada', content: json('Categoria') },
          400: erro('Nome inválido', 'Informe o nome da categoria (até 120 caracteres).'),
          401: SEM_TOKEN
        }
      }
    },

    '/categorias/{id}': {
      parameters: [ID],
      get: {
        tags: ['Categorias'],
        summary: 'Busca uma categoria',
        responses: {
          200: { description: 'Categoria encontrada', content: json('Categoria') },
          401: SEM_TOKEN,
          404: erro('Categoria não encontrada', 'Categoria não encontrada.')
        }
      },
      put: {
        tags: ['Categorias'],
        summary: 'Atualiza uma categoria',
        requestBody: { required: true, content: json('CategoriaEntrada') },
        responses: {
          200: { description: 'Categoria atualizada', content: json('Categoria') },
          400: erro('Nome inválido', 'Informe o nome da categoria (até 120 caracteres).'),
          401: SEM_TOKEN,
          404: erro('Categoria não encontrada', 'Categoria não encontrada.')
        }
      },
      delete: {
        tags: ['Categorias'],
        summary: 'Exclui uma categoria',
        responses: {
          204: { description: 'Categoria excluída (sem corpo)' },
          401: SEM_TOKEN,
          404: erro('Categoria não encontrada', 'Categoria não encontrada.'),
          409: erro('Categoria ainda tem produtos', 'Esta categoria ainda tem produtos e não pode ser excluída.')
        }
      }
    },

    // ---------- Produtos ----------

    '/produtos': {
      get: {
        tags: ['Produtos'],
        summary: 'Lista os produtos',
        parameters: [NOME],
        responses: {
          200: { description: 'Lista de produtos', content: listaJson('Produto') },
          401: SEM_TOKEN
        }
      },
      post: {
        tags: ['Produtos'],
        summary: 'Cria um produto',
        requestBody: { required: true, content: json('ProdutoEntrada') },
        responses: {
          201: { description: 'Produto criado', content: json('Produto') },
          400: erro('Dados inválidos', 'Categoria inexistente.'),
          401: SEM_TOKEN
        }
      }
    },

    '/produtos/{id}': {
      parameters: [ID],
      get: {
        tags: ['Produtos'],
        summary: 'Busca um produto',
        responses: {
          200: { description: 'Produto encontrado', content: json('Produto') },
          401: SEM_TOKEN,
          404: erro('Produto não encontrado', 'Produto não encontrado.')
        }
      },
      put: {
        tags: ['Produtos'],
        summary: 'Atualiza um produto (envie todos os campos)',
        requestBody: { required: true, content: json('ProdutoEntrada') },
        responses: {
          200: { description: 'Produto atualizado', content: json('Produto') },
          400: erro('Dados inválidos', 'Informe um preço válido, maior ou igual a zero.'),
          401: SEM_TOKEN,
          404: erro('Produto não encontrado', 'Produto não encontrado.')
        }
      },
      delete: {
        tags: ['Produtos'],
        summary: 'Exclui um produto',
        responses: {
          204: { description: 'Produto excluído (sem corpo)' },
          401: SEM_TOKEN,
          404: erro('Produto não encontrado', 'Produto não encontrado.')
        }
      }
    },

    // ---------- Usuários ----------

    '/usuarios': {
      get: {
        tags: ['Usuários'],
        summary: 'Lista os usuários',
        parameters: [NOME],
        responses: {
          200: { description: 'Lista de usuários (sem senha)', content: listaJson('Usuario') },
          401: SEM_TOKEN
        }
      },
      post: {
        tags: ['Usuários'],
        summary: 'Cadastra um usuário (pública)',
        description: 'Sem token, não é permitido criar um Administrador (tipo 11111111-1111-1111-1111-111111111111).',
        // Pública, mas aceita token: com ele, é possível criar um Administrador
        security: [{}, { bearerAuth: [] }],
        requestBody: { required: true, content: json('UsuarioCriar') },
        responses: {
          201: { description: 'Usuário criado (sem senha)', content: json('Usuario') },
          400: erro('Dados inválidos', 'Informe a senha.'),
          403: erro('Administrador sem token', 'Somente um usuário logado pode criar um Administrador.'),
          409: erro('E-mail repetido', 'Já existe um usuário com este e-mail.')
        }
      }
    },

    '/usuarios/{id}': {
      parameters: [ID],
      get: {
        tags: ['Usuários'],
        summary: 'Busca um usuário',
        responses: {
          200: { description: 'Usuário encontrado (sem senha)', content: json('Usuario') },
          401: SEM_TOKEN,
          404: erro('Usuário não encontrado', 'Usuário não encontrado.')
        }
      },
      put: {
        tags: ['Usuários'],
        summary: 'Atualiza um usuário',
        requestBody: { required: true, content: json('UsuarioAtualizar') },
        responses: {
          200: { description: 'Usuário atualizado (sem senha)', content: json('Usuario') },
          400: erro('Dados inválidos', 'Tipo de usuário inexistente.'),
          401: SEM_TOKEN,
          404: erro('Usuário não encontrado', 'Usuário não encontrado.'),
          409: erro('E-mail de outro usuário', 'Já existe um usuário com este e-mail.')
        }
      },
      delete: {
        tags: ['Usuários'],
        summary: 'Exclui um usuário',
        responses: {
          204: { description: 'Usuário excluído (sem corpo)' },
          401: SEM_TOKEN,
          404: erro('Usuário não encontrado', 'Usuário não encontrado.')
        }
      }
    }
  }
}
