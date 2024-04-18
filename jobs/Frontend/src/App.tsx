import { Outlet } from 'react-router-dom';
import Layout from './components/Layout.tsx';

export default function App() {
    return (
        <Layout>
            <Outlet />
        </Layout>
    );
}
