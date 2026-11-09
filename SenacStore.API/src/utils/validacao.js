const FORMATO_GUID = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i

// Um ID fora do formato não existe no banco. Conferir antes evita uma consulta inútil.
export function guidValido(valor) {
  return typeof valor === 'string' && FORMATO_GUID.test(valor)
}

// Texto preenchido e que cabe na coluna do banco
export function textoValido(valor, tamanhoMaximo) {
  return typeof valor === 'string' && valor.trim() !== '' && valor.trim().length <= tamanhoMaximo
}

export function emailValido(valor) {
  return textoValido(valor, 150) && /^\S+@\S+\.\S+$/.test(valor.trim())
}

// Monta o filtro do parâmetro ?nome=. Sem termo, a listagem traz tudo.
export function filtroPorNome(nome) {
  if (typeof nome !== 'string' || nome === '') {
    return {}
  }

  // No LIKE do MySQL, % e _ são curingas e \ é o caractere de escape.
  // Com uma \ na frente, eles valem como texto comum.
  return { nome: { contains: nome.replace(/[\\%_]/g, '\\$&') } }
}
