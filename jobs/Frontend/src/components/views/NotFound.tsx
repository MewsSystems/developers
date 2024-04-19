import Layout from '../shared/Layout';
import { Link } from 'react-router-dom';

export function NotFound() {
    return (
        <Layout>
            <h2>Page not found, <Link to="/">go home</Link></h2>
        </Layout>
    );
}