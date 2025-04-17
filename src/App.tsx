import './App.css';
import Home from './components/Home/Home';
import Login from './components/Login/Login';
import { useAuth } from './contexts/AuthContext';

function App() {
  const { user } = useAuth();
  return (

    <div className="App">
      {user ? (
        <Home />
      ) : (
        <Login />
      )}
    </div>
  );
}

export default App;
