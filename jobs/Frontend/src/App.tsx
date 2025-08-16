import { Outlet } from 'react-router-dom';
import Layout from './components/shared/Layout';

export default function App() {
    return (
        <Layout>
            <Outlet/>
        </Layout>
    );
}
