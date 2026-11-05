import TipoUsuario from '../models/TipoUsuario.js'
import { textoValido } from '../utils/validacao.js'

const NAO_ENCONTRADO = { mensagem: 'Tipo de usuário não encontrado.' }
const NOME_INVALIDO = { mensagem: 'Informe o nome do tipo de usuário (até 100 caracteres).' }

class TipoUsuarioController {

  constructor() {
    this.tipoUsuario = new TipoUsuario()
  }

  all = async (req, res) => {
    const tipos = await this.tipoUsuario.all(req.query.nome)
    return res.status(200).json(tipos)
  }

  show = async (req, res) => {
    const tipo = await this.tipoUsuario.show(req.params.id)

    if (!tipo) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    return res.status(200).json(tipo)
  }

  create = async (req, res) => {
    if (!textoValido(req.body.nome, 100)) {
      return res.status(400).json(NOME_INVALIDO)
    }

    const tipo = await this.tipoUsuario.create(req.body.nome.trim())
    return res.status(201).json(tipo)
  }

  update = async (req, res) => {
    if (!(await this.tipoUsuario.show(req.params.id))) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    if (!textoValido(req.body.nome, 100)) {
      return res.status(400).json(NOME_INVALIDO)
    }

    const tipo = await this.tipoUsuario.update(req.params.id, req.body.nome.trim())
    return res.status(200).json(tipo)
  }

  delete = async (req, res) => {
    if (!(await this.tipoUsuario.show(req.params.id))) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    if (await this.tipoUsuario.totalUsuarios(req.params.id) > 0) {
      return res.status(409).json({ mensagem: 'Este tipo ainda tem usuários e não pode ser excluído.' })
    }

    await this.tipoUsuario.delete(req.params.id)
    return res.status(204).end()
  }

}

export default new TipoUsuarioController()
