import Categoria from '../models/Categoria.js'
import { textoValido } from '../utils/validacao.js'

const NAO_ENCONTRADA = { mensagem: 'Categoria não encontrada.' }
const NOME_INVALIDO = { mensagem: 'Informe o nome da categoria (até 120 caracteres).' }

class CategoriaController {

  constructor() {
    this.categoria = new Categoria()
  }

  all = async (req, res) => {
    const categorias = await this.categoria.all(req.query.nome)
    return res.status(200).json(categorias)
  }

  show = async (req, res) => {
    const categoria = await this.categoria.show(req.params.id)

    if (!categoria) {
      return res.status(404).json(NAO_ENCONTRADA)
    }

    return res.status(200).json(categoria)
  }

  create = async (req, res) => {
    if (!textoValido(req.body.nome, 120)) {
      return res.status(400).json(NOME_INVALIDO)
    }

    const categoria = await this.categoria.create(req.body.nome.trim())
    return res.status(201).json(categoria)
  }

  update = async (req, res) => {
    if (!(await this.categoria.show(req.params.id))) {
      return res.status(404).json(NAO_ENCONTRADA)
    }

    if (!textoValido(req.body.nome, 120)) {
      return res.status(400).json(NOME_INVALIDO)
    }

    const categoria = await this.categoria.update(req.params.id, req.body.nome.trim())
    return res.status(200).json(categoria)
  }

  delete = async (req, res) => {
    if (!(await this.categoria.show(req.params.id))) {
      return res.status(404).json(NAO_ENCONTRADA)
    }

    if (await this.categoria.totalProdutos(req.params.id) > 0) {
      return res.status(409).json({ mensagem: 'Esta categoria ainda tem produtos e não pode ser excluída.' })
    }

    await this.categoria.delete(req.params.id)
    return res.status(204).end()
  }

}

export default new CategoriaController()
