import { PrismaMssql } from '@prisma/adapter-mssql'
import { PrismaClient } from '../generated/prisma/index.js'

// Adaptador exigido pelo Prisma 7 para falar com o SQL Server
const adapter = new PrismaMssql({
  server: process.env.DATABASE_HOST,
  port: Number(process.env.DATABASE_PORT ?? 1433),
  database: process.env.DATABASE_DB,
  user: process.env.DATABASE_USER,
  password: process.env.DATABASE_PASS,
  // O Azure SQL gratuito pausa quando fica parado e leva alguns segundos para acordar
  connectionTimeout: 30000,
  requestTimeout: 30000,
  options: {
    encrypt: true, // Obrigatório no Azure SQL
    // true só em SQL Server local, que usa certificado próprio. No Azure, deixe false.
    trustServerCertificate: process.env.DATABASE_TRUST_CERT === 'true'
  }
})

// Uma única instância para a API toda: cada PrismaClient abre o seu conjunto de conexões
export const prisma = new PrismaClient({ adapter })
