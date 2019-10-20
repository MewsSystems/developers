import axios from 'axios'

const instanse = axios.create({
    baseURL: 'http://localhost:3000',
})

export default instanse