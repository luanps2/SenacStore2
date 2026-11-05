import { Router } from 'express'
import CategoriaController from '../controllers/categoriaController.js'
import { autenticar } from '../middlewares/auth.js'
import { validarId } from '../middlewares/validarId.js'

const categoriaRouter = Router()

// Todas as rotas de categorias exigem token
categoriaRouter.use(autenticar)

categoriaRouter.get('/', CategoriaController.all)
categoriaRouter.get('/:id', validarId, CategoriaController.show)
categoriaRouter.post('/', CategoriaController.create)
categoriaRouter.put('/:id', validarId, CategoriaController.update)
categoriaRouter.delete('/:id', validarId, CategoriaController.delete)

export default categoriaRouter
