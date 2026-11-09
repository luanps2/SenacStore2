import { PrismaMariaDb } from '@prisma/adapter-mariadb'
import { PrismaClient } from '../generated/prisma/index.js'

// Adaptador exigido pelo Prisma 7 para falar com o MySQL (o mesmo pacote atende MySQL e MariaDB)
const adapter = new PrismaMariaDb({
  host: process.env.DATABASE_HOST,
  port: Number(process.env.DATABASE_PORT ?? 3306),
  database: process.env.DATABASE_DB,
  user: process.env.DATABASE_USER,
  password: process.env.DATABASE_PASS,
  connectionLimit: 5,
  connectTimeout: 30000,
  // O MySQL 8 usa o login caching_sha2_password. Sem SSL, o driver precisa buscar a chave pública do servidor.
  allowPublicKeyRetrieval: true,
  // O Azure Database for MySQL exige conexão segura. No MySQL local, deixe DATABASE_SSL=false.
  ssl: process.env.DATABASE_SSL === 'true' ? { rejectUnauthorized: true } : undefined
})

// Uma única instância para a API toda: cada PrismaClient abre o seu conjunto de conexões
export const prisma = new PrismaClient({ adapter })
