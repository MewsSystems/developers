import { useState, useEffect } from 'react'

export default useHttpHelper => {

    const [ error, setError ] = useState(null)

    const reqInterceptor = useHttpHelper.interceptors.request.use(
        req => {
            setError(null)
            return req
        })
    const resInterceptor = useHttpHelper.interceptors.response.use(
        res => res,
        err => {
            setError(err)
        })

    useEffect(() => {
        useHttpHelper.interceptors.request.eject(reqInterceptor)
        useHttpHelper.interceptors.response.eject(resInterceptor)
    }, [useHttpHelper.interceptors.request, reqInterceptor, useHttpHelper.interceptors.response, resInterceptor])

    const errorConfirmedHandler  = () => {
        setError(null)
    }

    return [ error, errorConfirmedHandler]

}