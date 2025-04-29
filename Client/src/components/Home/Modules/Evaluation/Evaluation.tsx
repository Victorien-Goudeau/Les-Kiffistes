// components/Evaluation/Evaluation.tsx
import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./Evaluation.css";
import { useApi } from "../../../../customs/useApi";

export interface RemediationModule {
    label: string;
    lesson: string;
    question: {
        prompt: string;
        choices: string;
        correctAnswer: string;
        explanationChoices: string[];
    };
}

interface Question {
    id: string;
    content: string;
    choices: string;
    correctAnswers: string;
    isUserAnswerCorrectly: boolean;
}

interface Quiz {
    id: string;
    title: string;
    questions: Question[];
}

interface SubmitQuizDto {
    quizId: string;
    questions: {
        id: string;
        isUserAnswerCorrectly: boolean;
    }[];
}

export default function Evaluation() {
    const { callApi } = useApi();
    const { id: urlId } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [quiz, setQuiz] = useState<Quiz | null>(null);
    const [questionIndex, setQuestionIndex] = useState(0);
    const [score, setScore] = useState<number | null>(null);

    // Charger le quiz depuis l'API (URL seulement pour le premier fetch)
    useEffect(() => {
        callApi("GET", `quiz/${urlId}`)
            .then(res => res.json())
            .then(data => setQuiz(data))
            .catch(err => console.error("Erreur chargement quiz :", err));
    }, [urlId, callApi]);

    const handleNextQuestion = async () => {
        if (!quiz) return;
        const name = `question${questionIndex}`;
        const selected = document.querySelector(
            `input[name="${name}"]:checked`
        ) as HTMLInputElement | null;
        if (!selected) {
            alert("Veuillez sélectionner une réponse avant de continuer.");
            return;
        }

        // Marquer la réponse
        const updatedQuestions = quiz.questions.map((q, idx) =>
            idx === questionIndex
                ? { ...q, isUserAnswerCorrectly: selected.value === q.correctAnswers }
                : q
        );
        const updatedQuiz = { ...quiz, questions: updatedQuestions };
        setQuiz(updatedQuiz);
        selected.checked = false;

        // S'il reste des questions
        if (questionIndex < updatedQuiz.questions.length - 1) {
            setQuestionIndex(i => i + 1);
            return;
        }

        // Sinon, on soumet
        const payload: SubmitQuizDto = {
            quizId: updatedQuiz.id,        // <-- utiliser quiz.id
            questions: updatedQuiz.questions.map(q => ({
                id: q.id,
                isUserAnswerCorrectly: q.isUserAnswerCorrectly,
            })),
        };

        try {
            const res = await callApi(
                "POST",
                `quiz/${updatedQuiz.id}/submit`,  // <-- quiz.id ici aussi
                JSON.stringify(payload)
            );
            const resultScore = (await res.json()) as number;
            setScore(resultScore);
        } catch (err) {
            console.error("Erreur soumission quiz :", err);
            alert("⚠️ Problème réseau ou serveur, réessayez plus tard.");
        }
    };

    const handleGenerateModule = async () => {
        if (!quiz) return;

        try {
            const res = await callApi(
                "POST",
                `remediation?quizId=${quiz.id}`     // <-- quiz.id et non urlId
            );
            if (!res.ok) throw new Error("Échec génération");
            const data = (await res.json()) as { modules: RemediationModule[] };

            // On navigue vers /remediation en passant l'objet complet
            navigate("/remediation", { state: data });
        } catch (err) {
            console.error("Erreur génération module :", err);
            alert("⚠️ Problème réseau ou serveur, réessayez plus tard.");
        }
    };

    if (!quiz) {
        return <div>Loading...</div>;
    }

    // Affiche les questions tant que le score n'existe pas
    if (score === null) {
        const current = quiz.questions[questionIndex];
        return (
            <div className="evaluation-module">
                <h1>Évaluation : {quiz.title}</h1>

                <div className="question-container">
                    <div className="question-container-header">
                        <h2>
                            Question {questionIndex + 1} / {quiz.questions.length}
                        </h2>
                    </div>

                    <div className="question-container-body">
                        <p>{current.content}</p>
                        <div className="responses-container">
                            {current.choices.split("|").map((choice, idx) => (
                                <div className="response-option" key={idx}>
                                    <input
                                        type="radio"
                                        id={`option${idx}`}
                                        name={`question${questionIndex}`}
                                        value={choice}
                                    />
                                    <label htmlFor={`option${idx}`} className="option-label">
                                        {choice}
                                    </label>
                                </div>
                            ))}
                        </div>
                    </div>

                    <div className="question-container-footer">
                        <button className="submit-button" onClick={handleNextQuestion}>
                            {questionIndex < quiz.questions.length - 1 ? "Next" : "Finish"}
                        </button>
                    </div>
                </div>
            </div>
        );
    }

    // Affiche le score + bouton Generate Module
    return (
        <div className="evaluation-module">
            <h1>Votre score : {score.toFixed(2)}%</h1>
            <button className="submit-button" onClick={handleGenerateModule}>
                Generate Module
            </button>
        </div>
    );
}
