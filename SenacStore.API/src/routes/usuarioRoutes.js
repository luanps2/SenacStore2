import { Router } from 'express'
import UsuarioController from '../controllers/usuarioController.js'
import { autenticar } from '../middlewares/auth.js'
import { validarId } from '../middlewares/validarId.js'

const usuarioRouter = Router()

// Pública: é o cadastro feito antes do login
usuarioRouter.post('/', UsuarioController.create)

// Daqui para baixo, todas as rotas exigem token
usuarioRouter.use(autenticar)

usuarioRouter.get('/', UsuarioController.all)
usuarioRouter.get('/:id', validarId, UsuarioController.show)
usuarioRouter.put('/:id', validarId, UsuarioController.update)
usuarioRouter.delete('/:id', validarId, UsuarioController.delete)

export default usuarioRouter
