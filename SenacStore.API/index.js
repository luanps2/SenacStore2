import express from 'express'
import swaggerUi from 'swagger-ui-express'
import { swaggerSpec } from './src/docs/swagger.js'
import authRoutes from './src/routes/authRoutes.js'
import tipoUsuarioRoutes from './src/routes/tipoUsuarioRoutes.js'
import categoriaRoutes from './src/routes/categoriaRoutes.js'
import produtoRoutes from './src/routes/produtoRoutes.js'
import usuarioRoutes from './src/routes/usuarioRoutes.js'
import { rotaNaoEncontrada, tratarErros } from './src/middlewares/erros.js'

const app = express()

app.use(express.json())

// Sem corpo na requisição, o Express deixa req.body indefinido. Assim os controllers leem {}.
app.use((req, res, next) => {
  req.body ??= {}
  next()
})

// Confirma que a API está no ar, sem consultar o banco.
app.get('/', (req, res) => {
  res.status(200).json({ mensagem: 'API SenacStore no ar.' })
})

app.use('/auth', authRoutes)
app.use('/tipos-usuario', tipoUsuarioRoutes)
app.use('/categorias', categoriaRoutes)
app.use('/produtos', produtoRoutes)
app.use('/usuarios', usuarioRoutes)

// Documentação interativa: lista as rotas e permite testá-las pelo navegador
app.use('/docs', swaggerUi.serve, swaggerUi.setup(swaggerSpec, {
  customSiteTitle: 'API SenacStore',
  swaggerOptions: { persistAuthorization: true } // O token continua lá ao recarregar a página
}))

// Estes dois ficam por último: só são alcançados se nada acima responder
app.use(rotaNaoEncontrada)
app.use(tratarErros)

export default app
