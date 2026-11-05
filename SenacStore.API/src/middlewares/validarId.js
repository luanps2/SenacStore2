import { guidValido } from '../utils/validacao.js'

// Um ID fora do formato GUID não existe no banco, então a resposta é 404
export function validarId(req, res, next) {
  if (!guidValido(req.params.id)) {
    return res.status(404).json({ mensagem: 'Registro não encontrado.' })
  }

  next()
}
