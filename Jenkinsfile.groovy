pipeline {
    agent { label 'windows-node' }
    
    stages {
        stage('Check Environment') {
            steps {
                bat 'echo Hello from windows-node!'
                bat 'git --version'
            }
        }
    }
}
