pipeline {
    agent {
        docker.image('unityci/editor:6000.0.28f1-webgl-3').inside('-v /root/.cache/unity3d:/root/.cache/unity3d -v /root/.local:/root/.local') {
            sh '''
            unity-editor \
            -batchmode \
            -nographics \
            -quit \
            -projectPath /workspace \
            -username "$UNITY_USERNAME" \
            -password "$UNITY_PASSWORD" \
            -serial "$UNITY_LICENSE"
            '''
        }
    }
    environment {
        UNITY_LICENSE = credentials('unity-license-key')
        PROJECT_PATH = 'C:\\jenkins_home\\workspace\\Unity\\AssassinsChrisGame'
        UNITY_CACHE = '/root/.cache/unity3d'
        UNITY_LOCAL = '/root/.local'
    }
    stages {
        stage('Activate License') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'unity-credentials', usernameVariable: 'UNITY_USERNAME', passwordVariable: 'UNITY_PASSWORD')]) {
                    echo 'Ativando licença do Unity...'
                    sh """
                    unity-editor \
                    -batchmode \
                    -logFile /dev/stdout \
                    -username ${UNITY_USERNAME} \
                    -password ${UNITY_PASSWORD} \
                    -serial ${UNITY_LICENSE}
                    """
                }
            }
        }
        stage('Run Tests') {
            steps {
                sh """
                unity-editor \
                -quit -batchmode \
                -projectPath ${PROJECT_PATH} \
                -executeMethod TestRunner.RunTests \
                -logFile /dev/stdout
                """
            }
        }
        stage('Build WebGL') {
            steps {
                sh """
                unity-editor \
                -quit -batchmode \
                -projectPath ${PROJECT_PATH} \
                -executeMethod BuildScript.BuildWebGL \
                -logFile /dev/stdout
                """
            }
        }
    }
    post {
        success {
            archiveArtifacts artifacts: 'Builds/**', allowEmptyArchive: true
        }
        failure {
            echo 'Build falhou. Verifique os logs.'
        }
    }
}
