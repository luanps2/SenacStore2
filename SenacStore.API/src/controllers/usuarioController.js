import bcrypt from 'bcryptjs'
import Usuario from '../models/Usuario.js'
import TipoUsuario from '../models/TipoUsuario.js'
import { lerToken } from '../middlewares/auth.js'
import { emailValido, guidValido, textoValido } from '../utils/validacao.js'

const NAO_ENCONTRADO = { mensagem: 'Usuário não encontrado.' }
const EMAIL_REPETIDO = { mensagem: 'Já existe um usuário com este e-mail.' }
const TIPO_ADMINISTRADOR = '11111111-1111-1111-1111-111111111111'

class UsuarioController {

  constructor() {
    this.usuario = new Usuario()
    this.tipoUsuario = new TipoUsuario()
  }

  // Regras comuns ao POST e ao PUT. Devolve o texto do erro, ou null se estiver tudo certo.
  validar = async ({ nome, email, tipoUsuarioId }) => {
    if (!textoValido(nome, 150)) {
      return 'Informe o nome do usuário (até 150 caracteres).'
    }

    if (!emailValido(email)) {
      return 'Informe um e-mail válido (até 150 caracteres).'
    }

    if (!guidValido(tipoUsuarioId) || !(await this.tipoUsuario.show(tipoUsuarioId))) {
      return 'Tipo de usuário inexistente.'
    }

    return null
  }

  all = async (req, res) => {
    const usuarios = await this.usuario.all(req.query.nome)
    return res.status(200).json(usuarios)
  }

  show = async (req, res) => {
    const usuario = await this.usuario.show(req.params.id)

    if (!usuario) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    return res.status(200).json(usuario)
  }

  // Rota pública: é o cadastro feito antes do login
  create = async (req, res) => {
    const { nome, email, senha, tipoUsuarioId } = req.body

    const erro = await this.validar(req.body)
    if (erro) {
      return res.status(400).json({ mensagem: erro })
    }

    if (typeof senha !== 'string' || senha === '') {
      return res.status(400).json({ mensagem: 'Informe a senha.' })
    }

    // Sem esta regra, qualquer pessoa criaria um administrador pela rota pública
    if (tipoUsuarioId === TIPO_ADMINISTRADOR && !lerToken(req)) {
      return res.status(403).json({ mensagem: 'Somente um usuário logado pode criar um Administrador.' })
    }

    if (await this.usuario.porEmail(email.trim())) {
      return res.status(409).json(EMAIL_REPETIDO)
    }

    const usuario = await this.usuario.create({
      nome: nome.trim(),
      email: email.trim(),
      senha: await bcrypt.hash(senha, 10), // Nunca grava a senha em texto puro
      tipoUsuarioId
    })

    return res.status(201).json(usuario)
  }

  update = async (req, res) => {
    const { id } = req.params
    const { nome, email, senha, tipoUsuarioId } = req.body

    if (!(await this.usuario.show(id))) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    const erro = await this.validar(req.body)
    if (erro) {
      return res.status(400).json({ mensagem: erro })
    }

    if (senha !== undefined && senha !== null && typeof senha !== 'string') {
      return res.status(400).json({ mensagem: 'A senha deve ser um texto.' })
    }

    // O e-mail pode continuar o mesmo, mas não pode ser o de outro usuário
    const dono = await this.usuario.porEmail(email.trim())
    if (dono && dono.id.toLowerCase() !== id.toLowerCase()) {
      return res.status(409).json(EMAIL_REPETIDO)
    }

    const dados = { nome: nome.trim(), email: email.trim(), tipoUsuarioId }

    // Senha ausente ou vazia mantém a atual
    if (senha) {
      dados.senha = await bcrypt.hash(senha, 10)
    }

    const usuario = await this.usuario.update(id, dados)
    return res.status(200).json(usuario)
  }

  delete = async (req, res) => {
    if (!(await this.usuario.show(req.params.id))) {
      return res.status(404).json(NAO_ENCONTRADO)
    }

    await this.usuario.delete(req.params.id)
    return res.status(204).end()
  }

}

export default new UsuarioController()
