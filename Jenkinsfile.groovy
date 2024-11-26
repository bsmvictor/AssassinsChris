pipeline {
    agent { label 'windows-node' }

    environment {
        UNITY_PATH = 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.24f1\\Editor\\Unity.exe'
        PROJECT_PATH = 'C:\\jenkins\\workspace\\AssassinsChris'
    //EMAIL_RECIPIENTS = credentials('email-recipients')
    }

    parameters {
        booleanParam(name: 'Windows_build', defaultValue: true, description: 'Executar Build para Windows')
        booleanParam(name: 'WebGL_build', defaultValue: true, description: 'Executar Build para WebGL')
    }

    stages {
        stage('bla') {
            steps {
                bat 'mkdir C:\\jenkins\\workspace\\AssassinsChris\\Builds\\TestDir'
                bat 'echo Test > C:\\jenkins\\workspace\\AssassinsChris\\Builds\\TestDir\\testfile.txt'
            }

        }
        stage('Run Tests') {
            steps {
                bat """
                "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod TestReportGenerator.RunTestsAndGenerateReport -logFile -
                """
            }
        }

         stage('Build Windows') {
            when({expression {params.Windows_build}})
             steps {
                 echo "Build Windows executada em: ${new Date()}"
                 bat """
                 "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod BuildScript.BuildWindows -logFile -
                 """
             }
        }

        stage('Build WebGL') {
            when({expression {params.WebGL_build}})
            steps {
                echo "Build WebGL executada em: ${new Date()}"
                bat """
                "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod BuildScript.BuildWebGL -logFile -
                """
            }
        }
    }
    
    post {
        always {
            archiveArtifacts artifacts: '**/*.zip', allowEmptyArchive: true
            archiveArtifacts artifacts: '**/TestResults/*.xml', allowEmptyArchive: true
        }
    }
}