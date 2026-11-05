import { prisma } from '../config/prisma.js'
import { filtroPorNome } from '../utils/validacao.js'

// O Prisma devolve Decimal, que viraria texto no JSON. O contrato pede número.
function comPrecoNumerico(produto) {
  return produto && { ...produto, preco: Number(produto.preco) }
}

export default class Produto {

  async all(nome) {
    const produtos = await prisma.produto.findMany({
      where: filtroPorNome(nome),
      orderBy: { nome: 'asc' }
    })

    return produtos.map(comPrecoNumerico)
  }

  async show(id) {
    return comPrecoNumerico(await prisma.produto.findUnique({ where: { id } }))
  }

  async create(dados) {
    return comPrecoNumerico(await prisma.produto.create({ data: dados }))
  }

  async update(id, dados) {
    return comPrecoNumerico(await prisma.produto.update({ where: { id }, data: dados }))
  }

  delete(id) {
    return prisma.produto.delete({ where: { id } })
  }

}
