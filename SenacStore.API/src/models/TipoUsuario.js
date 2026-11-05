import { prisma } from '../config/prisma.js'
import { filtroPorNome } from '../utils/validacao.js'

export default class TipoUsuario {

  all(nome) {
    return prisma.tipoUsuario.findMany({
      where: filtroPorNome(nome),
      orderBy: { nome: 'asc' }
    })
  }

  show(id) {
    return prisma.tipoUsuario.findUnique({ where: { id } })
  }

  create(nome) {
    return prisma.tipoUsuario.create({ data: { nome } })
  }

  update(id, nome) {
    return prisma.tipoUsuario.update({ where: { id }, data: { nome } })
  }

  delete(id) {
    return prisma.tipoUsuario.delete({ where: { id } })
  }

  // Usado para bloquear a exclusão de um tipo em uso
  totalUsuarios(id) {
    return prisma.usuario.count({ where: { tipoUsuarioId: id } })
  }

}
