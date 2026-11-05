import jwt from 'jsonwebtoken'

// Devolve os dados do token, ou null se ele estiver ausente, inválido ou expirado
export function lerToken(req) {
  const [tipo, token] = (req.headers.authorization ?? '').split(' ')

  if (tipo !== 'Bearer' || !token) {
    return null
  }

  try {
    return jwt.verify(token, process.env.JWT_SECRET, { algorithms: ['HS256'] })
  } catch {
    return null
  }
}

// Protege uma rota: sem token válido, a requisição para aqui
export function autenticar(req, res, next) {
  if (!lerToken(req)) {
    return res.status(401).json({ mensagem: 'Sua sessão expirou. Faça login novamente.' })
  }

  next()
}
