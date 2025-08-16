import { RouterProvider } from 'react-router-dom';
import router from './core/constants/router';
import Footer from './shared/components/Footer/Footer';
import GlobalContainer from './shared/components/GlobalContainer/GlobalContainer';
import Header from './shared/components/Header/Header';
import MainContent from './shared/components/MainContent/MainContent';
import GlobalStyle from './shared/styles/components/GlobalStyle';

const App = () => {
    return (
        <>
            <GlobalStyle />
            <GlobalContainer>
                <Header />
                <MainContent>
                    <RouterProvider router={router} />
                </MainContent>
                <Footer />
            </GlobalContainer>
        </>
    );
};

export default App;
