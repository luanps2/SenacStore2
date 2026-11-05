import { Router } from 'express'
import TipoUsuarioController from '../controllers/tipoUsuarioController.js'
import { autenticar } from '../middlewares/auth.js'
import { validarId } from '../middlewares/validarId.js'

const tipoUsuarioRouter = Router()

// Pública: preenche o combo da tela de cadastro, que abre antes do login
tipoUsuarioRouter.get('/', TipoUsuarioController.all)

// Daqui para baixo, todas as rotas exigem token
tipoUsuarioRouter.use(autenticar)

tipoUsuarioRouter.get('/:id', validarId, TipoUsuarioController.show)
tipoUsuarioRouter.post('/', TipoUsuarioController.create)
tipoUsuarioRouter.put('/:id', validarId, TipoUsuarioController.update)
tipoUsuarioRouter.delete('/:id', validarId, TipoUsuarioController.delete)

export default tipoUsuarioRouter
