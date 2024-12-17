import Footer from "@/components/Footer/Footer";
import Header from "@/components/Header/Header";
import { AppContainer } from "@/layouts/MainStyle";

interface MainProps {
  children: React.ReactNode;
}

const Main: React.FC<MainProps> = ({ children }) => {
  return (
    <>
      <Header />
      <AppContainer>{children}</AppContainer>
      <Footer />
    </>
  );
};

export default Main;
