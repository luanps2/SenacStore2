// Rota que não existe: responde em JSON, e não com a página HTML padrão do Express
export function rotaNaoEncontrada(req, res) {
  return res.status(404).json({ mensagem: 'Rota não encontrada.' })
}

// Os controllers já conferem os dados antes de gravar. Estes códigos só aparecem
// quando duas requisições disputam o mesmo registro ao mesmo tempo.
function erroDoPrisma(err, req) {
  switch (err.code) {
    case 'P2000':
      return { status: 400, mensagem: 'Um dos campos é maior do que o permitido.' }
    case 'P2002':
      return { status: 409, mensagem: 'Já existe um registro com estes dados.' }
    case 'P2003':
      return req.method === 'DELETE'
        ? { status: 409, mensagem: 'Este registro está em uso e não pode ser excluído.' }
        : { status: 400, mensagem: 'Um dos IDs informados não existe.' }
    case 'P2025':
      return { status: 404, mensagem: 'Registro não encontrado.' }
    default:
      return null
  }
}

// O Express 5 entrega aqui qualquer erro de um controller async.
// Precisa dos 4 parâmetros para ser reconhecido como middleware de erro.
export function tratarErros(err, req, res, next) {
  if (err.type === 'entity.parse.failed') {
    return res.status(400).json({ mensagem: 'O corpo da requisição não é um JSON válido.' })
  }

  if (err.type === 'entity.too.large') {
    return res.status(413).json({ mensagem: 'Os dados enviados são grandes demais.' })
  }

  const erro = erroDoPrisma(err, req)
  if (erro) {
    return res.status(erro.status).json({ mensagem: erro.mensagem })
  }

  // O detalhe fica só no log do servidor; o cliente nunca recebe dados internos
  console.error(err)
  return res.status(500).json({ mensagem: 'Erro interno no servidor.' })
}
