import { Router } from 'express'
import ProdutoController from '../controllers/produtoController.js'
import { autenticar } from '../middlewares/auth.js'
import { validarId } from '../middlewares/validarId.js'

const produtoRouter = Router()

// Todas as rotas de produtos exigem token
produtoRouter.use(autenticar)

produtoRouter.get('/', ProdutoController.all)
produtoRouter.get('/:id', validarId, ProdutoController.show)
produtoRouter.post('/', ProdutoController.create)
produtoRouter.put('/:id', validarId, ProdutoController.update)
produtoRouter.delete('/:id', validarId, ProdutoController.delete)

export default produtoRouter
