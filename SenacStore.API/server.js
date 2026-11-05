import 'dotenv/config'
import app from './index.js'

// Sem estas variáveis a API não funciona. É melhor parar agora, com um aviso claro.
const obrigatorias = ['JWT_SECRET', 'DATABASE_HOST', 'DATABASE_DB', 'DATABASE_USER', 'DATABASE_PASS']
const faltando = obrigatorias.filter(nome => !process.env[nome])

if (faltando.length > 0) {
  console.error(`Configure estas variáveis de ambiente: ${faltando.join(', ')}`)
  process.exit(1)
}

// O Azure informa a porta em PORT. Na máquina local, usa a 3000.
const port = process.env.PORT ?? 3000

app.listen(port, () => {
  console.log(`API SenacStore rodando na porta ${port}`)
})
