import AppRouterProvider from './core/providers/AppRouterProvider';

const App = () => {
    return (
        <div className="app">
            <header>The MovieDB Search app</header>
            <main>
                <AppRouterProvider />
            </main>
            <footer>Footer</footer>
        </div>
    );
};

export default App;
