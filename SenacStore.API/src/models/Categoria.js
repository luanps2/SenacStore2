import { prisma } from '../config/prisma.js'
import { filtroPorNome } from '../utils/validacao.js'

export default class Categoria {

  all(nome) {
    return prisma.categoria.findMany({
      where: filtroPorNome(nome),
      orderBy: { nome: 'asc' }
    })
  }

  show(id) {
    return prisma.categoria.findUnique({ where: { id } })
  }

  create(nome) {
    return prisma.categoria.create({ data: { nome } })
  }

  update(id, nome) {
    return prisma.categoria.update({ where: { id }, data: { nome } })
  }

  delete(id) {
    return prisma.categoria.delete({ where: { id } })
  }

  // Usado para bloquear a exclusão de uma categoria em uso
  totalProdutos(id) {
    return prisma.produto.count({ where: { categoriaId: id } })
  }

}
