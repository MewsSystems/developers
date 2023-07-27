import { Link } from "react-router-dom";

export default function NavigationBar() {

  
  return (
    <div>
      <nav>
        <ul>
          <li>
            <Link to='/'>Search</Link>
          </li>
          <li>
            <Link to='/movies/1'>About</Link>
          </li>
          <li>
            <Link to='/dashboard'>Dashboard</Link>
          </li>
          <li>
            <Link to='/nothing-here'>Nothing Here</Link>
          </li>
        </ul>
      </nav>
    </div>
  );
}
