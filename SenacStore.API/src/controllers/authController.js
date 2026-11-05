import bcrypt from 'bcryptjs'
import jwt from 'jsonwebtoken'
import Usuario from '../models/Usuario.js'
import { textoValido } from '../utils/validacao.js'

class AuthController {

  constructor() {
    this.usuario = new Usuario()
  }

  login = async (req, res) => {
    const { email, senha } = req.body

    if (!textoValido(email, 150) || typeof senha !== 'string' || senha === '') {
      return res.status(400).json({ mensagem: 'Informe e-mail e senha.' })
    }

    const usuario = await this.usuario.porEmail(email.trim())

    // A mesma mensagem nos dois casos: a API não revela quais e-mails estão cadastrados
    if (!usuario || !(await bcrypt.compare(senha, usuario.senha))) {
      return res.status(401).json({ mensagem: 'E-mail ou senha inválidos.' })
    }

    const token = jwt.sign({ id: usuario.id }, process.env.JWT_SECRET, { expiresIn: '8h' })

    // Separa a senha do restante, para responder sem ela
    const { senha: _hash, ...semSenha } = usuario

    return res.status(200).json({ token, usuario: semSenha })
  }

}

export default new AuthController()
