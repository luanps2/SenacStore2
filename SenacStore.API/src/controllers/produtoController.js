import Produto from '../models/Produto.js'
import Categoria from '../models/Categoria.js'
import { guidValido, textoValido } from '../utils/validacao.js'

const NAO_ENCONTRADO = { mensagem: 'Produto não encontrado.' }
const PRECO_MAXIMO = 99999999.99 // Limite da coluna DECIMAL(10,2)

class ProdutoController {

  constructor() {
    this.produto = new Produto()
    this.categoria = new Categoria()
  }

  // Devolve o texto do erro, ou null se os dados estiverem corretos
  validar = async ({ nome, preco, categoriaId }) => {
    if (!textoValido(nome, 150)) {
      return 'Informe o nome do produto (até 150 caracteres).'
    }

    if (typeof preco !== 'number' || !(preco >= 0 && preco <= PRECO_MAXIMO)) {
      return 'Informe um preço válido, maior ou igual a zero.'
    }

    if (!guidValido(categoriaId) || !(await this.categoria.show(categoriaId))) {
      return 'Categoria inexistente.'
    }

    return null
  }

  all = async (req, res) => {
    const produtos = await this.produto.all(req.query.nome)
    return res.status(200).json(produtos)
  }

  show = async (req, res) => {
    const produto = await this.produto.show(req.params.id)

    if (!produto) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    return res.status(200).json(produto)
  }

  create = async (req, res) => {
    const erro = await this.validar(req.body)
    if (erro) {
      return res.status(400).json({ mensagem: erro })
    }

    const { nome, preco, categoriaId } = req.body
    const produto = await this.produto.create({ nome: nome.trim(), preco, categoriaId })

    return res.status(201).json(produto)
  }

  update = async (req, res) => {
    if (!(await this.produto.show(req.params.id))) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    const erro = await this.validar(req.body)
    if (erro) {
      return res.status(400).json({ mensagem: erro })
    }

    const { nome, preco, categoriaId } = req.body
    const produto = await this.produto.update(req.params.id, { nome: nome.trim(), preco, categoriaId })

    return res.status(200).json(produto)
  }

  delete = async (req, res) => {
    if (!(await this.produto.show(req.params.id))) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    await this.produto.delete(req.params.id)
    return res.status(204).end()
  }

}

export default new ProdutoController()
