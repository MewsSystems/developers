export default function Footer() {
    return (
        <footer className="border-t border-white/10">
            <div className="container py-6 text-xs text-neutral-400">
                © {new Date().getFullYear()} CinEmir — Built with React +
                Tailwind
            </div>
        </footer>
    );
}
