import Footer from '@/components/Footer';
import Header from '@/components/Header';
import Loading from '@/pages/loading-tmp';
import { Suspense } from 'react';
import { Outlet } from 'react-router-dom';

const Layout = () => {
  return (
    <div className="grid grid-rows-[auto_1fr_auto] min-h-screen">
      <Header />
      <main className="w-full max-w-7xl mx-auto p-8 md:p-14">
        <Suspense fallback={<Loading />}>
          <Outlet />
        </Suspense>
      </main>
      <Footer />
    </div>
  );
};

export default Layout;
