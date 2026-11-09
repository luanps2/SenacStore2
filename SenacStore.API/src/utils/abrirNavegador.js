import { spawn } from 'node:child_process'
import { existsSync, writeFileSync } from 'node:fs'
import { tmpdir } from 'node:os'
import { join } from 'node:path'

// Só abre o navegador na máquina do desenvolvedor: nunca no Azure (que define WEBSITE_SITE_NAME),
// nem em produção, nem quando ABRIR_NAVEGADOR=false no .env
function deveAbrir() {
  return !process.env.WEBSITE_SITE_NAME
    && process.env.NODE_ENV !== 'production'
    && process.env.ABRIR_NAVEGADOR !== 'false'
}

// No "npm run dev" (node --watch), a API reinicia a cada arquivo salvo. Sem esta conferência,
// uma aba nova abriria a cada reinício. O processo que observa os arquivos (process.ppid) é o
// mesmo em todos os reinícios, então um arquivo marcador com o número dele indica que a aba já abriu.
function jaAbriuNesteWatch() {
  if (!process.env.WATCH_REPORT_DEPENDENCIES) {
    return false
  }

  const marcador = join(tmpdir(), `senacstore-api-navegador-${process.ppid}`)
  if (existsSync(marcador)) {
    return true
  }

  writeFileSync(marcador, '')
  return false
}

export function abrirNavegador(url) {
  if (!deveAbrir() || jaAbriuNesteWatch()) {
    return
  }

  // Cada sistema operacional tem o seu comando para abrir uma URL no navegador padrão
  const [comando, args] =
    process.platform === 'win32' ? ['cmd', ['/c', 'start', '', url]]
      : process.platform === 'darwin' ? ['open', [url]]
        : ['xdg-open', [url]]

  // Se não conseguir abrir (servidor sem interface gráfica, por exemplo), a API segue normalmente
  spawn(comando, args, { stdio: 'ignore', detached: true })
    .on('error', () => {})
    .unref()
}
