import { useAuth } from '@/entities/auth/api/providers/AuthProvider'
import { Box, Button, Flex } from '@chakra-ui/react'
import { Link, Outlet, useNavigate, useRouter } from '@tanstack/react-router'


export function AuthLayout() {
  const router = useRouter()
  const navigate = useNavigate()
  const auth = useAuth()

  const handleLogout = () => {
    if (window.confirm('Are you sure you want to logout?')) {
      auth.logout().then(() => {
        router.invalidate().finally(() => {
          navigate({ to: '/' })
        })
      })
    }
  }
  return (
    <div >
      <Flex justifyContent="space-between">
        <Link to="/movies" className="[&.active]:font-bold">
          Search movies
        </Link>{' '}

        <Box>
          <Button
            size="md"
            onClick={handleLogout}
          >
            Logout
          </Button>

        </Box>
      </Flex>
      <Outlet />
    </div>
  )
}
