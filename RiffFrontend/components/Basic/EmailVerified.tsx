'use client'

import { useEffect } from 'react'
import { useRouter, useSearchParams } from 'next/navigation'
import { toast } from 'react-hot-toast'

export default function EmailVerificationHandler() {
  const router = useRouter()
  const searchParams = useSearchParams()

  useEffect(() => {
    const status = searchParams.get('email')
    if (status) {
      if (status === 'verified') {
        toast.success('Email успешно подтверждён!')
      } else if (status === 'error') {
        toast.error('Не удалось подтвердить email')
      }

      router.replace('/', { scroll: false })
    }
  }, [searchParams, router])

  return null
}