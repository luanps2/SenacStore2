import { prisma } from '../config/prisma.js'
import { filtroPorNome } from '../utils/validacao.js'

// "omit" tira o hash da senha do resultado, para ele nunca sair em uma resposta
const SEM_SENHA = { senha: true }

export default class Usuario {

  all(nome) {
    return prisma.usuario.findMany({
      where: filtroPorNome(nome),
      omit: SEM_SENHA,
      orderBy: { nome: 'asc' }
    })
  }

  show(id) {
    return prisma.usuario.findUnique({ where: { id }, omit: SEM_SENHA })
  }

  // Único método que traz o hash da senha: é usado só para conferir o login e o e-mail repetido
  porEmail(email) {
    return prisma.usuario.findUnique({ where: { email } })
  }

  create(dados) {
    return prisma.usuario.create({ data: dados, omit: SEM_SENHA })
  }

  update(id, dados) {
    return prisma.usuario.update({ where: { id }, data: dados, omit: SEM_SENHA })
  }

  delete(id) {
    return prisma.usuario.delete({ where: { id } })
  }

}
